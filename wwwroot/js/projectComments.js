// projectComments.js - Handles AJAX functionality for project comments

$(document).ready(function () {
    // Load comments when the page loads
    const projectId = $('#projectId').val();
    if (projectId) {
        loadComments(projectId);
    }

    // Handle comment form submission
    $('#commentForm').submit(function (e) {
        e.preventDefault();
        addComment();
    });
});

// Function to load comments for a project
function loadComments(projectId) {
    $.ajax({
        url: `/api/ProjectComment/GetComments/${projectId}`,
        type: 'GET',
        success: function (data) {
            displayComments(data);
        },
        error: function (error) {
            console.error('Error loading comments:', error);
            $('#comments-container').html('<p class="text-danger">Error loading comments. Please try again later.</p>');
        }
    });
}

// Function to add a new comment
function addComment() {
    const projectId = $('#projectId').val();
    const content = $('#commentContent').val();

    if (!content.trim()) {
        alert('Please enter a comment');
        return;
    }

    const commentData = {
        projectId: parseInt(projectId),
        content: content
    };

    $.ajax({
        url: '/api/ProjectComment/AddComment',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(commentData),
        success: function (data) {
            // Clear the form
            $('#commentContent').val('');
            
            // Reload comments
            loadComments(projectId);
        },
        error: function (error) {
            console.error('Error adding comment:', error);
            alert('Error adding comment. Please try again.');
        }
    });
}

// Function to display comments
function displayComments(comments) {
    const container = $('#comments-container');
    
    if (comments.length === 0) {
        container.html('<p>No comments yet. Be the first to comment!</p>');
        return;
    }
    
    let html = '';
    
    comments.forEach(comment => {
        const date = new Date(comment.createdDate).toLocaleString();
        html += `
            <div class="comment mb-3 p-3 border rounded">
                <p class="mb-1">${escapeHtml(comment.content)}</p>
                <small class="text-muted">Posted on ${date}</small>
            </div>
        `;
    });
    
    container.html(html);
}

// Helper function to escape HTML to prevent XSS
function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
} 