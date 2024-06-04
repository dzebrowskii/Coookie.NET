$(document).ready(function() {
    $('.friend-link').click(function(event) {
        event.preventDefault();
        var userId = $(this).data('userid');
        $.get('/Friends/FriendDetails', { userId: userId }, function(data) {
            $('#friendDetailsModal .modal-content').html(data);
            $('#friendDetailsModal').modal('show');
        }).fail(function() {
            alert("Error loading friend details. Please try again later.");
        });
    });
});
