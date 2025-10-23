// API Base URL
const API_BASE = window.location.origin;

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    loadDocumentCount();
    setupEventListeners();
});

function setupEventListeners() {
    // Chat form submit
    document.getElementById('chatForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        const input = document.getElementById('userInput');
        const question = input.value.trim();
        
        if (question) {
            await askQuestion(question);
            input.value = '';
        }
    });

    // Add fruit form submit
    document.getElementById('addFruitForm').addEventListener('submit', async (e) => {
        e.preventDefault();
        await addFruit(e.target);
    });
}

// Load document count
async function loadDocumentCount() {
    try {
        const response = await fetch(`${API_BASE}/api/health`);
        const data = await response.json();
        document.getElementById('docCount').innerHTML = `
            <i class="fas fa-file-alt mr-1"></i>${data.documents_count} documents
        `;
    } catch (error) {
        console.error('Error loading document count:', error);
    }
}

// Ask question
async function askQuestion(question) {
    const chatContainer = document.getElementById('chatContainer');
    
    // Clear welcome message if exists
    const welcomeMsg = chatContainer.querySelector('.text-center.py-12');
    if (welcomeMsg) {
        welcomeMsg.remove();
    }
    
    // Add user message
    addMessage(question, 'user');
    
    // Add typing indicator
    const typingId = addTypingIndicator();
    
    try {
        const response = await fetch(`${API_BASE}/api/chat`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                question: question,
                top_k: 3
            })
        });
        
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        
        const data = await response.json();
        
        // Remove typing indicator
        removeTypingIndicator(typingId);
        
        // Add AI response
        addMessage(data.answer, 'ai', data.sources);
        
    } catch (error) {
        removeTypingIndicator(typingId);
        addMessage('Xin lỗi, đã có lỗi xảy ra. Vui lòng thử lại sau.', 'ai');
        console.error('Error:', error);
    }
}

// Add message to chat
function addMessage(text, sender, sources = null) {
    const chatContainer = document.getElementById('chatContainer');
    const messageDiv = document.createElement('div');
    messageDiv.className = `message mb-4 flex ${sender === 'user' ? 'justify-end' : 'justify-start'}`;
    
    let sourcesHtml = '';
    if (sources && sources.length > 0) {
        sourcesHtml = `
            <div class="mt-2 pt-2 border-t border-gray-300 text-xs">
                <span class="font-semibold">Nguồn: </span>
                ${sources.map(s => s.fruit_name).join(', ')}
            </div>
        `;
    }
    
    messageDiv.innerHTML = `
        <div class="max-w-[70%]">
            <div class="${sender === 'user' ? 'chat-bubble-user text-white' : 'chat-bubble-ai'} px-4 py-3 rounded-2xl shadow-md">
                <div class="whitespace-pre-wrap">${escapeHtml(text)}</div>
                ${sourcesHtml}
            </div>
            <div class="text-xs text-gray-500 mt-1 ${sender === 'user' ? 'text-right' : 'text-left'}">
                ${new Date().toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' })}
            </div>
        </div>
    `;
    
    chatContainer.appendChild(messageDiv);
    chatContainer.scrollTop = chatContainer.scrollHeight;
}

// Add typing indicator
function addTypingIndicator() {
    const chatContainer = document.getElementById('chatContainer');
    const typingDiv = document.createElement('div');
    const id = 'typing-' + Date.now();
    typingDiv.id = id;
    typingDiv.className = 'message mb-4 flex justify-start';
    typingDiv.innerHTML = `
        <div class="max-w-[70%]">
            <div class="chat-bubble-ai px-4 py-3 rounded-2xl shadow-md">
                <div class="typing-indicator">
                    <span></span>
                    <span></span>
                    <span></span>
                </div>
            </div>
        </div>
    `;
    chatContainer.appendChild(typingDiv);
    chatContainer.scrollTop = chatContainer.scrollHeight;
    return id;
}

// Remove typing indicator
function removeTypingIndicator(id) {
    const element = document.getElementById(id);
    if (element) {
        element.remove();
    }
}

// Escape HTML
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// Show data manager modal
function showDataManager() {
    document.getElementById('dataModal').classList.remove('hidden');
    loadFruitsList();
}

// Close data manager modal
function closeDataManager() {
    document.getElementById('dataModal').classList.add('hidden');
}

// Show tab
function showTab(tabName) {
    // Hide all tabs
    document.querySelectorAll('.tab-content').forEach(tab => {
        tab.classList.add('hidden');
    });
    
    // Remove active class from all tab buttons
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.classList.remove('border-purple-500', 'text-purple-600');
        btn.classList.add('text-gray-500');
    });
    
    // Show selected tab
    document.getElementById(tabName + 'Content').classList.remove('hidden');
    
    // Add active class to selected tab button
    const activeBtn = document.getElementById(tabName + 'Tab');
    activeBtn.classList.add('border-purple-500', 'text-purple-600');
    activeBtn.classList.remove('text-gray-500');
    
    // Load data if viewing
    if (tabName === 'view') {
        loadFruitsList();
    }
}

// Load fruits list
async function loadFruitsList() {
    const container = document.getElementById('fruitsListContainer');
    container.innerHTML = '<p class="text-gray-500 text-center py-8">Đang tải dữ liệu...</p>';
    
    try {
        const response = await fetch(`${API_BASE}/api/fruits`);
        const fruits = await response.json();
        
        if (fruits.length === 0) {
            container.innerHTML = '<p class="text-gray-500 text-center py-8">Chưa có dữ liệu</p>';
            return;
        }
        
        container.innerHTML = fruits.map(fruit => `
            <div class="border border-gray-200 rounded-lg p-4 hover:shadow-md transition">
                <div class="flex items-start justify-between">
                    <div class="flex-1">
                        <h3 class="text-lg font-bold text-gray-800">${fruit.fruit_name}</h3>
                        <p class="text-sm text-gray-600 mt-1">${fruit.description}</p>
                        <div class="mt-2 flex flex-wrap gap-2">
                            <span class="text-xs bg-purple-100 text-purple-700 px-2 py-1 rounded">
                                <i class="fas fa-calendar mr-1"></i>${fruit.season}
                            </span>
                            <span class="text-xs bg-green-100 text-green-700 px-2 py-1 rounded">
                                <i class="fas fa-utensils mr-1"></i>${fruit.usage}
                            </span>
                        </div>
                    </div>
                    <div class="ml-4">
                        <span class="text-2xl">🍎</span>
                    </div>
                </div>
                <div class="mt-3 pt-3 border-t border-gray-200">
                    <details class="text-sm">
                        <summary class="cursor-pointer font-semibold text-purple-600 hover:text-purple-700">
                            Xem chi tiết
                        </summary>
                        <div class="mt-2 space-y-2">
                            <div>
                                <span class="font-semibold">Thành phần dinh dưỡng:</span>
                                <ul class="ml-4 mt-1 text-gray-600">
                                    ${Object.entries(fruit.nutrients).map(([key, value]) => 
                                        `<li>• ${key.replace(/_/g, ' ')}: ${value}</li>`
                                    ).join('')}
                                </ul>
                            </div>
                            <div>
                                <span class="font-semibold">Lợi ích sức khỏe:</span>
                                <ul class="ml-4 mt-1 text-gray-600">
                                    ${fruit.health_benefits.map(benefit => 
                                        `<li>• ${benefit}</li>`
                                    ).join('')}
                                </ul>
                            </div>
                        </div>
                    </details>
                </div>
            </div>
        `).join('');
        
    } catch (error) {
        container.innerHTML = '<p class="text-red-500 text-center py-8">Lỗi khi tải dữ liệu</p>';
        console.error('Error loading fruits:', error);
    }
}

// Add fruit
async function addFruit(form) {
    const formData = new FormData(form);
    
    try {
        // Parse nutrients JSON
        const nutrients = JSON.parse(formData.get('nutrients'));
        
        // Parse health benefits (split by newline)
        const healthBenefits = formData.get('health_benefits')
            .split('\n')
            .map(line => line.trim())
            .filter(line => line.length > 0);
        
        const fruitData = {
            id: formData.get('id'),
            fruit_name: formData.get('fruit_name'),
            description: formData.get('description'),
            season: formData.get('season'),
            usage: formData.get('usage'),
            nutrients: nutrients,
            health_benefits: healthBenefits
        };
        
        const response = await fetch(`${API_BASE}/api/train/add`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(fruitData)
        });
        
        if (!response.ok) {
            throw new Error('Failed to add fruit');
        }
        
        const result = await response.json();
        
        // Show success message
        alert(result.message);
        
        // Reset form
        form.reset();
        
        // Reload document count
        loadDocumentCount();
        
        // Switch to view tab
        showTab('view');
        
    } catch (error) {
        alert('Lỗi: ' + error.message);
        console.error('Error adding fruit:', error);
    }
}

// Reload data
async function reloadData() {
    const statusDiv = document.getElementById('reloadStatus');
    statusDiv.innerHTML = '<p class="text-blue-600"><i class="fas fa-spinner fa-spin mr-2"></i>Đang reload...</p>';
    
    try {
        const response = await fetch(`${API_BASE}/api/train/reload`, {
            method: 'POST'
        });
        
        if (!response.ok) {
            throw new Error('Failed to reload data');
        }
        
        const result = await response.json();
        
        statusDiv.innerHTML = `<p class="text-green-600"><i class="fas fa-check-circle mr-2"></i>${result.message}</p>`;
        
        // Reload document count
        loadDocumentCount();
        
        // Auto hide status after 3 seconds
        setTimeout(() => {
            statusDiv.innerHTML = '';
        }, 3000);
        
    } catch (error) {
        statusDiv.innerHTML = `<p class="text-red-600"><i class="fas fa-exclamation-circle mr-2"></i>Lỗi: ${error.message}</p>`;
        console.error('Error reloading data:', error);
    }
}

// Close modal when clicking outside
document.getElementById('dataModal')?.addEventListener('click', (e) => {
    if (e.target.id === 'dataModal') {
        closeDataManager();
    }
});
