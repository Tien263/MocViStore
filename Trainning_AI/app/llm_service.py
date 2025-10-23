from typing import List, Dict
from app.config import settings
import os


class LLMService:
    def __init__(self):
        """Khởi tạo LLM service - Ưu tiên Gemini (miễn phí) > OpenAI > Simple"""
        self.client = None
        self.gemini_model = None
        
        # Try Gemini first (FREE!)
        try:
            gemini_key = os.getenv("GEMINI_API_KEY", "")
            if gemini_key:
                import google.generativeai as genai
                genai.configure(api_key=gemini_key)
                # Use latest flash model (fast & free)
                self.gemini_model = genai.GenerativeModel('gemini-2.0-flash')
                print("[OK] Su dung Google Gemini 2.0 Flash (mien phi)")
                return
        except ImportError:
            print("[WARNING] Chua cai google-generativeai. Chay: pip install google-generativeai")
        except Exception as e:
            print(f"[WARNING] Gemini error: {e}")
        
        # Try OpenAI if Gemini not available
        try:
            if settings.OPENAI_API_KEY:
                from openai import OpenAI
                self.client = OpenAI(api_key=settings.OPENAI_API_KEY)
                print("[OK] Su dung OpenAI GPT")
                return
        except ImportError:
            pass
        
        print("[WARNING] Khong co AI API key. Su dung che do simple response.")
    
    def generate_response(self, query: str, context: List[Dict]) -> str:
        """Tạo câu trả lời dựa trên query và context"""
        # Check for casual conversation first (greetings, thanks, etc.)
        casual_response = self._handle_casual_conversation(query)
        if casual_response:
            return casual_response
        
        # Check if question is out of scope BEFORE checking context
        # This prevents AI from answering irrelevant questions even if vector search returns results
        out_of_scope_keywords = [
            'thời tiết', 'weather', 'tin tức', 'news', 'bóng đá', 'football',
            'chính trị', 'politics', 'âm nhạc', 'music', 'phim', 'movie',
            'game', 'xe', 'car', 'du lịch', 'travel', 'khách sạn', 'hotel',
            'máy tính', 'computer', 'điện thoại', 'phone', 'toán', 'math',
            'lịch sử', 'history', 'địa lý', 'geography', 'nấu ăn', 'cooking',
            'thể thao', 'sport', 'sách', 'book', 'học', 'study'
        ]
        
        query_lower = query.lower()
        if any(keyword in query_lower for keyword in out_of_scope_keywords):
            return self._generate_out_of_scope_response(query)
        
        # Check context quality - Use 0.6 threshold (balanced)
        has_good_context = context and len(context) > 0 and context[0]['distance'] < 0.6
        
        # If no good context, generate flexible out-of-scope response
        if not has_good_context:
            return self._generate_out_of_scope_response(query)
        
        # ALWAYS pass context to AI, let AI decide if it's relevant
        # Don't filter out context here
        
        # Try Gemini first
        if self.gemini_model:
            return self._generate_gemini_response(query, context)
        
        # Try OpenAI
        if self.client:
            return self._generate_openai_response(query, context)
        
        # Fallback to simple response
        return self._generate_simple_response(query, context)
    
    def _handle_casual_conversation(self, query: str) -> str:
        """Xử lý các câu chuyện phiếm, giao tiếp thân thiện"""
        query_lower = query.lower().strip()
        
        # Chào hỏi - Check exact words to avoid false matches (e.g., "bao nhiêu" contains "chào")
        greeting_words = ['xin chào', 'hello', 'hi', 'hey', 'chào bạn']
        # Check if query is ONLY greeting (not part of other words)
        if any(word == query_lower or query_lower.startswith(word + ' ') or query_lower.endswith(' ' + word) for word in greeting_words):
            return "Chào bạn! Mình là tư vấn viên của Mộc Vị đây. Rất vui được hỗ trợ bạn hôm nay! 😊 Bạn muốn tìm hiểu về loại hoa quả nào của shop nhỉ?"
        # Also check standalone "chào" but not when it's part of another word
        if query_lower == 'chào' or query_lower.startswith('chào ') or query_lower.endswith(' chào'):
            return "Chào bạn! Mình là tư vấn viên của Mộc Vị đây. Rất vui được hỗ trợ bạn hôm nay! 😊 Bạn muốn tìm hiểu về loại hoa quả nào của shop nhỉ?"
        
        # Cảm ơn
        if any(word in query_lower for word in ['cảm ơn', 'cám ơn', 'thank', 'thanks']):
            return "Không có gì đâu bạn! 😊 Mình luôn sẵn sàng tư vấn thêm nếu bạn cần nhé!"
        
        # Tạm biệt
        if any(word in query_lower for word in ['tạm biệt', 'bye', 'goodbye', 'bái bai']):
            return "Tạm biệt bạn nhé! Hẹn gặp lại! 👋 Chúc bạn một ngày tuyệt vời!"
        
        # Hỏi thăm - CHỈ khi hỏi về AI, KHÔNG phải về sản phẩm
        # Tránh false positive với "như thế nào", "ra sao" trong câu hỏi về sản phẩm
        health_patterns = ['bạn khỏe không', 'bạn thế nào', 'how are you', 'bạn có khỏe', 'bạn ổn không']
        if any(pattern in query_lower for pattern in health_patterns):
            return "Mình khỏe lắm, cảm ơn bạn đã hỏi thăm! 😊 Hôm nay bạn muốn tìm hiểu về sản phẩm nào của shop không?"
        
        # Giới thiệu bản thân
        if any(word in query_lower for word in ['bạn là ai', 'bạn là gì', 'who are you', 'giới thiệu']):
            return "Mình là tư vấn viên AI của Mộc Vị - chuyên về hoa quả sấy cao cấp từ Mộc Châu! 🍓 Shop mình có đủ loại: Dâu tây, Mận, Xoài, Đào, Hồng, Mít, Chuối, Sữa chua sấy. Bạn quan tâm loại nào nhất?"
        
        # Khen ngợi
        if any(word in query_lower for word in ['giỏi', 'tuyệt', 'hay', 'good job', 'amazing', 'pro']):
            return "Cảm ơn bạn nhiều nha! 🥰 Mình rất vui khi giúp được bạn. Còn thắc mắc gì cứ hỏi mình nhé!"
        
        # Không hiểu
        if any(word in query_lower for word in ['không hiểu', 'không rõ', "don't understand", 'chưa hiểu']):
            return "Ối, xin lỗi bạn nha! 😅 Để mình giải thích lại rõ hơn. Hoặc bạn có thể hỏi mình về:\n- Thành phần dinh dưỡng của từng loại quả\n- Giá cả và khuyến mãi\n- Lợi ích cho sức khỏe\n- Cách bảo quản và sử dụng"
        
        return None  # Không phải casual conversation
    
    def _generate_out_of_scope_response(self, query: str) -> str:
        """Tạo response linh hoạt khi câu hỏi nằm ngoài phạm vi"""
        
        # Default out-of-scope response
        default_response = """Ối, câu hỏi này hơi ngoài chuyên môn của mình rồi! 😅

Mình là tư vấn viên chuyên về hoa quả sấy Mộc Vị thôi nha. Mình có thể giúp bạn về:
🍓 Sản phẩm hoa quả sấy (Dâu tây, Mận, Xoài, Đào, Hồng, Mít, Chuối, Sữa chua)
💰 Giá cả và khuyến mãi
💪 Lợi ích sức khỏe
🎁 Gói quà tặng

Bạn muốn tìm hiểu về sản phẩm nào không? 😊"""
        
        # If Gemini available, use it for flexible response
        if self.gemini_model:
            try:
                prompt = f"""Bạn là tư vấn viên của Mộc Vị - shop hoa quả sấy Mộc Châu.

Khách hỏi: "{query}"

Câu hỏi này KHÔNG liên quan đến hoa quả sấy.

NHIỆM VỤ: Từ chối lịch sự, chuyển hướng về sản phẩm

YÊU CẦU:
✅ Ngắn gọn 2 câu
✅ Xin lỗi + Gợi ý về sản phẩm
✅ Dùng "mình", "bạn" (thân thiện)
✅ 1 emoji

VÍ DỤ:
"Ối, câu này mình không rành lắm! 😅 Nhưng mình có thể tư vấn bạn về hoa quả sấy Mộc Châu nha - bạn muốn biết về loại nào?"

Trả lời NGẮN:"""

                safety_settings = [
                    {"category": "HARM_CATEGORY_HARASSMENT", "threshold": "BLOCK_NONE"},
                    {"category": "HARM_CATEGORY_HATE_SPEECH", "threshold": "BLOCK_NONE"},
                    {"category": "HARM_CATEGORY_SEXUALLY_EXPLICIT", "threshold": "BLOCK_NONE"},
                    {"category": "HARM_CATEGORY_DANGEROUS_CONTENT", "threshold": "BLOCK_NONE"},
                ]
                
                response = self.gemini_model.generate_content(
                    prompt,
                    safety_settings=safety_settings,
                    generation_config={
                        "temperature": 0.7,
                        "max_output_tokens": 150  # Limit to ~3-4 sentences
                    }
                )
                
                return response.text.strip()
                
            except Exception as e:
                print(f"[WARNING] Gemini error in out-of-scope: {e}")
                # Fallback to default message
        
        # Fallback: Return default response
        return default_response
    
    def _generate_gemini_response(self, query: str, context: List[Dict]) -> str:
        """Tạo câu trả lời bằng Google Gemini"""
        try:
            context_text = self._format_context(context)
            
            prompt = f"""Bạn là nhân viên tư vấn của Mộc Vị - shop hoa quả sấy Mộc Châu.

THÔNG TIN SẢN PHẨM:
{context_text}

CÂU HỏI: {query}

QUY TẮC TRẢ LỜI:
✅ Phong cách: Tự nhiên, thân thiện, ngắn gọn
✅ Ngôn ngữ: Tiếng Việt đời thường (dùng "mình", "bạn", "nha", "nhé")
✅ Nội dung:
   - TRẢ LỜI ĐÚNG TRỌNG TÂM câu hỏi
   - Nếu hỏi GIÁ: Chỉ nói về giá, KHÔNG kể hết thông tin sản phẩm
   - Nếu hỏi về 1 loại quả: Giới thiệu ngắn gọn 2-3 điểm nổi bật
   - Nếu hỏi "các sản phẩm": Liệt kê tên + giá các loại
   - Dùng emoji vừa phải (1-2 emoji)
✅ Độ dài: TỐI ĐA 3-4 câu, KHÔNG viết dài
❌ TUYỆT ĐỐI TRÁNH:
   - Dump toàn bộ thông tin sản phẩm
   - Copy nguyên văn mô tả
   - Trả lời không đúng trọng tâm
   - Dùng từ "SIÊU PHẨM", "ĐỈNH CAO"

VÍ DỤ TỐT:
Q: "Giá mận bao nhiêu?"
A: "Mận sấy dẻo của mình giá 65k/200g hoặc 18k/gói mini 50g nha bạn! 🍑"

Q: "Các sản phẩm giá bao nhiêu?"
A: "Dạ shop mình có: Dâu tây dẻo 90k, Dâu thăng hoa 280k, Mận 65k, Xoài 75k, Đào 65k, Hồng 100k, Mít 80k, Chuối 80k, Sữa chua 180k (giá gói 200g nha). Còn gói mini 50g từ 18-75k tùy loại! 😊"

Trả lời NGẮN GỌN, ĐÚNG TRỌNG TÂM!"""

            # Use streaming for better UX
            # Set safety settings to BLOCK_NONE to avoid blocking responses
            safety_settings = [
                {"category": "HARM_CATEGORY_HARASSMENT", "threshold": "BLOCK_NONE"},
                {"category": "HARM_CATEGORY_HATE_SPEECH", "threshold": "BLOCK_NONE"},
                {"category": "HARM_CATEGORY_SEXUALLY_EXPLICIT", "threshold": "BLOCK_NONE"},
                {"category": "HARM_CATEGORY_DANGEROUS_CONTENT", "threshold": "BLOCK_NONE"},
            ]
            
            response = self.gemini_model.generate_content(
                prompt,
                stream=True,
                safety_settings=safety_settings
            )
            
            # Collect and print streaming response
            full_response = ""
            chunk_count = 0
            for chunk in response:
                chunk_count += 1
                if chunk.text:
                    print(chunk.text, end='', flush=True)
                    full_response += chunk.text
            
            # Debug: if no response, print error
            if not full_response:
                print(f"\n[DEBUG] Received {chunk_count} chunks but no text")
            
            return full_response
            
        except Exception as e:
            print(f"[ERROR] Gemini error: {e}")
            return self._generate_simple_response(query, context)
    
    def _generate_openai_response(self, query: str, context: List[Dict]) -> str:
        """Tạo câu trả lời bằng OpenAI"""
        try:
            context_text = self._format_context(context)
            
            system_prompt = """Bạn là chuyên gia về hoa quả Mộc Châu. 
Trả lời câu hỏi dựa trên thông tin được cung cấp.
Trả lời ngắn gọn, chính xác bằng tiếng Việt."""

            user_prompt = f"""Thông tin:

{context_text}

Câu hỏi: {query}"""

            response = self.client.chat.completions.create(
                model=settings.LLM_MODEL,
                messages=[
                    {"role": "system", "content": system_prompt},
                    {"role": "user", "content": user_prompt}
                ],
                temperature=0.7,
                max_tokens=500
            )
            
            return response.choices[0].message.content
        
        except Exception as e:
            print(f"[ERROR] OpenAI error: {e}")
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
        
        # Nếu độ liên quan < 20% -> Không biết (giảm ngưỡng để trả lời nhiều hơn)
        if relevance_score < 0.20:
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
