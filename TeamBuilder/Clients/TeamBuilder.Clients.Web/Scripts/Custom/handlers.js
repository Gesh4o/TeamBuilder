function onSendingInvitationClick() {
    $("#addFriendForm").submit();
    $("#add-user-btn").addClass("disabled");

    // Return false to prevent link from redirecting.
    return false;
}

function onFriendRequestSend(args) {
    console.log(args);
    console.log("Request send!");
}

function OnRemoveFriendClick() {
    $("#addFriendForm").submit();
    $("#remove-user-btn").addClass("disabled");

    // Return false to prevent link from redirecting.
    return false; 
}

function onFriendRemoveSuccess(args) {
    console.log(args);
    console.log("Friend removed.");
}