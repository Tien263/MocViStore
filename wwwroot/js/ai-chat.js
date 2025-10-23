/**
 * AI Chat Widget
 * Tích hợp với Mộc Châu Fruits AI API
 */

class AIChatWidget {
    constructor() {
        // API Configuration - Thay đổi URL này khi deploy AI server
        this.apiUrl = 'http://localhost:8000/api/chat';
        this.healthUrl = 'http://localhost:8000/api/health';
        
        this.isOpen = false;
        this.isTyping = false;
        this.messages = [];
        
        this.init();
    }
    
    init() {
        this.createWidget();
        this.attachEventListeners();
        this.checkAIHealth();
        
        // Load chat history from localStorage
        this.loadChatHistory();
    }
    
    createWidget() {
        const widgetHTML = `
            <div class="ai-chat-widget">
                <!-- Floating Button -->
                <button class="ai-chat-button" id="aiChatButton" title="Chat với AI">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                        <path d="M20 2H4c-1.1 0-2 .9-2 2v18l4-4h14c1.1 0 2-.9 2-2V4c0-1.1-.9-2-2-2zm0 14H6l-2 2V4h16v12z"/>
                        <path d="M7 9h2v2H7zm4 0h2v2h-2zm4 0h2v2h-2z"/>
                    </svg>
                    <span class="ai-chat-badge" id="aiChatBadge" style="display: none;">1</span>
                </button>
                
                <!-- Chat Window -->
                <div class="ai-chat-window" id="aiChatWindow">
                    <!-- Header -->
                    <div class="ai-chat-header">
                        <div class="ai-chat-header-info">
                            <div class="ai-chat-avatar">🤖</div>
                            <div class="ai-chat-title">
                                <h3>AI Mộc Châu</h3>
                                <div class="ai-chat-status">
                                    <span class="ai-chat-status-dot"></span>
                                    <span id="aiChatStatus">Đang kết nối...</span>
                                </div>
                            </div>
                        </div>
                        <div class="ai-chat-header-actions">
                            <button class="ai-chat-clear" id="aiChatClear" title="Xóa lịch sử chat">
                                <svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                                    <polyline points="3 6 5 6 21 6"></polyline>
                                    <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                                </svg>
                            </button>
                            <button class="ai-chat-close" id="aiChatClose" title="Đóng">×</button>
                        </div>
                    </div>
                    
                    <!-- Messages -->
                    <div class="ai-chat-messages" id="aiChatMessages">
                        <div class="ai-welcome-message">
                            <h4>👋 Xin chào!</h4>
                            <p>Tôi là trợ lý AI của Mộc Vị Store. Tôi có thể giúp bạn tìm hiểu về các loại hoa quả sấy Mộc Châu.</p>
                            <div class="ai-quick-questions">
                                <button class="ai-quick-question" data-question="Cho tôi biết về dâu tây sấy">
                                    🍓 Dâu tây sấy có gì đặc biệt?
                                </button>
                                <button class="ai-quick-question" data-question="Các loại hoa quả sấy nào tốt cho sức khỏe?">
                                    💪 Hoa quả nào tốt cho sức khỏe?
                                </button>
                                <button class="ai-quick-question" data-question="Giá của các sản phẩm như thế nào?">
                                    💰 Giá sản phẩm ra sao?
                                </button>
                            </div>
                        </div>
                    </div>
                    
                    <!-- Input -->
                    <div class="ai-chat-input-container">
                        <input 
                            type="text" 
                            class="ai-chat-input" 
                            id="aiChatInput" 
                            placeholder="Hỏi về hoa quả Mộc Châu..."
                            autocomplete="off"
                        />
                        <button class="ai-chat-send" id="aiChatSend" title="Gửi">
                            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24">
                                <path d="M2.01 21L23 12 2.01 3 2 10l15 2-15 2z"/>
                            </svg>
                        </button>
                    </div>
                </div>
            </div>
        `;
        
        document.body.insertAdjacentHTML('beforeend', widgetHTML);
    }
    
    attachEventListeners() {
        // Toggle chat window
        document.getElementById('aiChatButton').addEventListener('click', () => {
            this.toggleChat();
        });
        
        document.getElementById('aiChatClose').addEventListener('click', () => {
            this.toggleChat();
        });
        
        // Send message
        document.getElementById('aiChatSend').addEventListener('click', () => {
            this.sendMessage();
        });
        
        document.getElementById('aiChatInput').addEventListener('keypress', (e) => {
            if (e.key === 'Enter') {
                this.sendMessage();
            }
        });
        
        // Quick questions
        document.querySelectorAll('.ai-quick-question').forEach(btn => {
            btn.addEventListener('click', (e) => {
                const question = e.target.getAttribute('data-question');
                this.sendMessage(question);
            });
        });
        
        // Clear chat history
        document.getElementById('aiChatClear').addEventListener('click', () => {
            this.clearChatHistory();
        });
    }
    
    toggleChat() {
        this.isOpen = !this.isOpen;
        const chatWindow = document.getElementById('aiChatWindow');
        const chatButton = document.getElementById('aiChatButton');
        const chatBadge = document.getElementById('aiChatBadge');
        
        if (this.isOpen) {
            chatWindow.classList.add('active');
            chatButton.classList.add('active');
            chatBadge.style.display = 'none';
            document.getElementById('aiChatInput').focus();
        } else {
            chatWindow.classList.remove('active');
            chatButton.classList.remove('active');
        }
    }
    
    async checkAIHealth() {
        const statusElement = document.getElementById('aiChatStatus');
        
        try {
            const response = await fetch(this.healthUrl);
            const data = await response.json();
            
            if (data.status === 'healthy') {
                statusElement.textContent = 'Trực tuyến';
                statusElement.style.color = '#4ade80';
            } else {
                statusElement.textContent = 'Không khả dụng';
                statusElement.style.color = '#fbbf24';
            }
        } catch (error) {
            console.error('AI Health check failed:', error);
            statusElement.textContent = 'Ngoại tuyến';
            statusElement.style.color = '#ef4444';
            
            // Show notification badge
            document.getElementById('aiChatBadge').style.display = 'flex';
        }
    }
    
    async sendMessage(customMessage = null) {
        const input = document.getElementById('aiChatInput');
        const message = customMessage || input.value.trim();
        
        if (!message) return;
        
        // Clear input
        input.value = '';
        
        // Add user message
        this.addMessage('user', message);
        
        // Show typing indicator
        this.showTypingIndicator();
        
        try {
            // Call AI API
            const response = await fetch(this.apiUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    question: message,
                    top_k: 3
                })
            });
            
            if (!response.ok) {
                throw new Error('API request failed');
            }
            
            const data = await response.json();
            
            // Hide typing indicator
            this.hideTypingIndicator();
            
            // Add AI response
            this.addMessage('bot', data.answer, data.sources);
            
        } catch (error) {
            console.error('Error sending message:', error);
            this.hideTypingIndicator();
            
            // Show error message
            this.addMessage('bot', 
                '❌ Xin lỗi, tôi đang gặp sự cố kết nối. Vui lòng đảm bảo AI server đang chạy (python app/main.py) hoặc thử lại sau.',
                []
            );
        }
        
        // Save chat history
        this.saveChatHistory();
    }
    
    addMessage(type, content, sources = []) {
        const messagesContainer = document.getElementById('aiChatMessages');
        const time = new Date().toLocaleTimeString('vi-VN', { 
            hour: '2-digit', 
            minute: '2-digit' 
        });
        
        // Remove welcome message if exists
        const welcomeMsg = messagesContainer.querySelector('.ai-welcome-message');
        if (welcomeMsg) {
            welcomeMsg.remove();
        }
        
        const messageHTML = `
            <div class="ai-message ${type}">
                <div class="ai-message-avatar">
                    ${type === 'bot' ? '🤖' : '👤'}
                </div>
                <div class="ai-message-content">
                    <div class="ai-message-bubble">
                        ${this.formatMessage(content)}
                    </div>
                    <div class="ai-message-time">${time}</div>
                </div>
            </div>
        `;
        
        messagesContainer.insertAdjacentHTML('beforeend', messageHTML);
        
        // Add sources if available
        if (sources && sources.length > 0) {
            const sourcesHTML = `
                <div class="ai-message bot">
                    <div class="ai-message-avatar">📚</div>
                    <div class="ai-message-content">
                        <div class="ai-message-bubble" style="font-size: 12px; background: #f8f9fa; color: #6c757d;">
                            <strong>Nguồn tham khảo:</strong><br/>
                            ${sources.map((s, i) => 
                                `${i + 1}. ${s.fruit_name} (${(s.relevance_score * 100).toFixed(0)}%)`
                            ).join('<br/>')}
                        </div>
                    </div>
                </div>
            `;
            messagesContainer.insertAdjacentHTML('beforeend', sourcesHTML);
        }
        
        // Scroll to bottom
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
        
        // Store message
        this.messages.push({ type, content, time, sources });
    }
    
    formatMessage(text) {
        // Convert markdown-style formatting
        return text
            .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
            .replace(/\*(.*?)\*/g, '<em>$1</em>')
            .replace(/\n/g, '<br/>');
    }
    
    showTypingIndicator() {
        const messagesContainer = document.getElementById('aiChatMessages');
        const typingHTML = `
            <div class="ai-message bot" id="aiTypingIndicator">
                <div class="ai-message-avatar">🤖</div>
                <div class="ai-message-content">
                    <div class="ai-typing-indicator">
                        <div class="ai-typing-dot"></div>
                        <div class="ai-typing-dot"></div>
                        <div class="ai-typing-dot"></div>
                    </div>
                </div>
            </div>
        `;
        
        messagesContainer.insertAdjacentHTML('beforeend', typingHTML);
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
        
        // Disable input
        document.getElementById('aiChatInput').disabled = true;
        document.getElementById('aiChatSend').disabled = true;
    }
    
    hideTypingIndicator() {
        const indicator = document.getElementById('aiTypingIndicator');
        if (indicator) {
            indicator.remove();
        }
        
        // Enable input
        document.getElementById('aiChatInput').disabled = false;
        document.getElementById('aiChatSend').disabled = false;
        document.getElementById('aiChatInput').focus();
    }
    
    saveChatHistory() {
        try {
            localStorage.setItem('aiChatHistory', JSON.stringify(this.messages));
        } catch (error) {
            console.error('Error saving chat history:', error);
        }
    }
    
    loadChatHistory() {
        try {
            const history = localStorage.getItem('aiChatHistory');
            if (history) {
                this.messages = JSON.parse(history);
                
                // Restore messages (limit to last 10)
                const recentMessages = this.messages.slice(-10);
                if (recentMessages.length > 0) {
                    const messagesContainer = document.getElementById('aiChatMessages');
                    const welcomeMsg = messagesContainer.querySelector('.ai-welcome-message');
                    if (welcomeMsg) {
                        welcomeMsg.remove();
                    }
                    
                    recentMessages.forEach(msg => {
                        this.addMessage(msg.type, msg.content, msg.sources || []);
                    });
                }
            }
        } catch (error) {
            console.error('Error loading chat history:', error);
        }
    }
    
    clearChatHistory() {
        if (confirm('Bạn có chắc muốn xóa toàn bộ lịch sử chat?')) {
            // Clear messages array
            this.messages = [];
            
            // Clear localStorage
            localStorage.removeItem('aiChatHistory');
            
            // Clear UI
            const messagesContainer = document.getElementById('aiChatMessages');
            messagesContainer.innerHTML = `
                <div class="ai-welcome-message">
                    <h4>👋 Xin chào!</h4>
                    <p>Tôi là trợ lý AI của Mộc Vị Store. Tôi có thể giúp bạn tìm hiểu về các loại hoa quả sấy Mộc Châu.</p>
                    <div class="ai-quick-questions">
                        <button class="ai-quick-question" data-question="Cho tôi biết về dâu tây sấy">
                            🍓 Dâu tây sấy có gì đặc biệt?
                        </button>
                        <button class="ai-quick-question" data-question="Các loại hoa quả sấy nào tốt cho sức khỏe?">
                            💪 Hoa quả nào tốt cho sức khỏe?
                        </button>
                        <button class="ai-quick-question" data-question="Giá của các sản phẩm như thế nào?">
                            💰 Giá sản phẩm ra sao?
                        </button>
                    </div>
                </div>
            `;
            
            // Re-attach quick question listeners
            document.querySelectorAll('.ai-quick-question').forEach(btn => {
                btn.addEventListener('click', (e) => {
                    const question = e.target.getAttribute('data-question');
                    this.sendMessage(question);
                });
            });
            
            console.log('Chat history cleared!');
        }
    }
}

// Initialize widget when DOM is ready
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        new AIChatWidget();
    });
} else {
    new AIChatWidget();
}
