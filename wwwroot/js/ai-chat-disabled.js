/**
 * AI Chat Disabled Placeholder
 * This file ensures any cached AI chat functionality is completely disabled
 */

console.log('AI Chat is completely disabled - no functionality loaded');

// Prevent any AI chat initialization
window.AIChatWidget = function() {
    console.log('AI Chat widget creation blocked - feature disabled');
};

// Clean up any existing AI elements
document.addEventListener('DOMContentLoaded', function() {
    // Remove AI chat elements
    var aiElements = document.querySelectorAll('.ai-chat-widget, .ai-chat-button, .ai-chat-window');
    aiElements.forEach(function(element) {
        element.remove();
    });
    
    // Clear any AI-related localStorage
    try {
        localStorage.removeItem('aiChatSessionId');
        localStorage.removeItem('aiChatHistory');
    } catch (e) {
        // Ignore localStorage errors
    }
    
    console.log('AI Chat cleanup completed');
});
