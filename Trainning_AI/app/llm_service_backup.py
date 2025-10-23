from typing import List, Dict
from app.config import settings
import os


class LLMService:
    def __init__(self):
        """Khởi tạo LLM service"""
        self.client = None
        self.gemini_model = None
        
        # Try Gemini first (free)
        try:
            gemini_key = os.getenv("GEMINI_API_KEY", "")
            if gemini_key:
                import google.generativeai as genai
                genai.configure(api_key=gemini_key)
                self.gemini_model = genai.GenerativeModel('gemini-pro')
                print("✅ Sử dụng Google Gemini AI (miễn phí)")
                return
        except ImportError:
            pass
        except Exception as e:
            print(f"⚠️ Gemini error: {e}")
        
        # Try OpenAI if Gemini not available
        try:
            if settings.OPENAI_API_KEY:
                from openai import OpenAI
                self.client = OpenAI(api_key=settings.OPENAI_API_KEY)
                print("✅ Sử dụng OpenAI GPT")
                return
        except ImportError:
            pass
        
        print("⚠️ Không có AI API key. Sử dụng chế độ simple response.")
    
    def generate_response(self, query: str, context: List[Dict]) -> str:
        """
        Tạo câu trả lời dựa trên query và context từ vector store
        
        Args:
            query: Câu hỏi của người dùng
            context: Danh sách các documents liên quan từ vector store
        
        Returns:
            Câu trả lời được tạo bởi LLM
        """
        # Try Gemini first
        if self.gemini_model:
            return self._generate_gemini_response(query, context)
        
        # Try OpenAI
        if self.client:
            return self._generate_openai_response(query, context)
        
        # Fallback to simple response
        return self._generate_simple_response(query, context)
        
        # Tạo context string từ retrieved documents
        context_text = self._format_context(context)
        
        # Tạo prompt
        system_prompt = """Bạn là một chuyên gia về hoa quả Mộc Châu. 
Nhiệm vụ của bạn là trả lời câu hỏi của người dùng dựa trên thông tin được cung cấp.
Hãy trả lời một cách thân thiện, chính xác và chi tiết.
Nếu thông tin không có trong context, hãy nói rõ là bạn không có đủ thông tin để trả lời.
Luôn trả lời bằng tiếng Việt."""

        user_prompt = f"""Dựa trên thông tin sau đây:

{context_text}

Hãy trả lời câu hỏi: {query}"""

        try:
            # Gọi OpenAI API
            response = self.client.chat.completions.create(
                model=settings.LLM_MODEL,
                messages=[
                    {"role": "system", "content": system_prompt},
                    {"role": "user", "content": user_prompt}
                ],
                temperature=0.7,
                max_tokens=800
            )
            
            return response.choices[0].message.content
        
        except Exception as e:
            print(f"Error calling LLM: {e}")
            return self._generate_simple_response(query, context)
    
    def _format_context(self, context: List[Dict]) -> str:
        """Format context thành string"""
        if not context:
            return "Không tìm thấy thông tin liên quan."
        
        formatted = []
        for i, doc in enumerate(context, 1):
            formatted.append(f"--- Thông tin {i} ---")
            formatted.append(doc['content'])
            formatted.append("")
        
        return "\n".join(formatted)
    
    def _generate_simple_response(self, query: str, context: List[Dict]) -> str:
        """
        Tạo câu trả lời đơn giản khi không có LLM API
        Chỉ trả lời chính xác về quả được hỏi
        """
        if not context:
            return "Xin lỗi, tôi không tìm thấy thông tin liên quan đến câu hỏi của bạn. Vui lòng thử hỏi về các loại hoa quả Mộc Châu như: mận, mơ, dâu tây, táo, đào, nho."
        
        # Danh sách tên quả để kiểm tra (thứ tự quan trọng: từ dài đến ngắn)
        fruit_keywords = [
            ('dâu tây', 'Dâu Tây Mộc Châu'),
            ('mận', 'Mận Mộc Châu'),
            ('mơ', 'Mơ Mộc Châu'),
            ('táo', 'Táo Mộc Châu'),
            ('đào', 'Đào Mộc Châu'),
            ('nho', 'Nho Mộc Châu'),
            ('dâu', 'Dâu Tây Mộc Châu'),  # Để cuối để tránh match sai
        ]
        
        # Tìm quả được hỏi trong câu hỏi
        query_lower = query.lower()
        target_fruit = None
        
        for keyword, fruit_name in fruit_keywords:
            if keyword in query_lower:
                target_fruit = fruit_name
                break
        
        # Nếu tìm thấy tên quả cụ thể, tìm context khớp với quả đó
        best_match = None
        if target_fruit:
            for ctx in context:
                if ctx['metadata'].get('fruit_name') == target_fruit:
                    best_match = ctx
                    break
        
        # Nếu không tìm thấy context khớp, dùng context đầu tiên
        if not best_match:
            best_match = context[0]
        
        # Kiểm tra độ liên quan - nếu quá thấp thì từ chối
        relevance_score = 1 - best_match['distance']
        
        # Nếu độ liên quan < 35% -> Không biết
        if relevance_score < 0.35:
            return "Xin lỗi, tôi không có đủ thông tin để trả lời câu hỏi này. Tôi chỉ biết về các loại hoa quả Mộc Châu như: mận, mơ, dâu tây, táo, đào, nho. Bạn có thể hỏi về thành phần dinh dưỡng, lợi ích sức khỏe, mùa vụ hoặc cách sử dụng của các loại hoa quả này."
        
        # Trả lời chỉ về quả được hỏi
        fruit_name = best_match['metadata'].get('fruit_name', 'hoa quả')
        
        # Parse nội dung để trả lời ngắn gọn hơn
        content = best_match['content']
        
        # Nếu hỏi về thành phần/vitamin/dinh dưỡng
        if any(keyword in query.lower() for keyword in ['thành phần', 'vitamin', 'dinh dưỡng', 'chất', 'khoáng']):
            # Tìm phần thành phần dinh dưỡng
            lines = content.split('\n')
            nutrients_section = []
            in_nutrients = False
            
            for line in lines:
                if 'Thành phần dinh dưỡng' in line:
                    in_nutrients = True
                    continue
                if in_nutrients:
                    if line.strip().startswith('-'):
                        nutrients_section.append(line)
                    elif 'Lợi ích' in line or 'Mùa' in line or 'Cách' in line:
                        break
            
            if nutrients_section:
                return f"**{fruit_name}** có các thành phần dinh dưỡng sau:\n" + '\n'.join(nutrients_section)
        
        # Nếu hỏi về lợi ích/tác dụng
        elif any(keyword in query.lower() for keyword in ['lợi ích', 'tác dụng', 'tốt cho', 'giúp']):
            lines = content.split('\n')
            benefits_section = []
            in_benefits = False
            
            for line in lines:
                if 'Lợi ích sức khỏe' in line:
                    in_benefits = True
                    continue
                if in_benefits:
                    if line.strip().startswith('-'):
                        benefits_section.append(line)
                    elif 'Mùa' in line or 'Cách' in line:
                        break
            
            if benefits_section:
                return f"**{fruit_name}** có các lợi ích sức khỏe:\n" + '\n'.join(benefits_section)
        
        # Nếu hỏi về mùa vụ
        elif any(keyword in query.lower() for keyword in ['mùa', 'tháng', 'khi nào']):
            for line in content.split('\n'):
                if 'Mùa vụ:' in line:
                    season = line.replace('Mùa vụ:', '').strip()
                    return f"**{fruit_name}** có mùa vụ: {season}"
        
        # Trả lời chung (toàn bộ thông tin)
        return f"Thông tin về **{fruit_name}**:\n\n{content}"
