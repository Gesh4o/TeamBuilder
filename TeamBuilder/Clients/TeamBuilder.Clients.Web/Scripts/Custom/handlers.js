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

// -- USER settings.
// Handle change PICTURE.
function onProfilePictureUpdateSuccess(response) {
    let parsedResponse = JSON.parse(response);
    swal("Good job!", parsedResponse.message, "success");
    clearForm('update-picture-form');
}

function onProfilePictureUpdateFail(data) {
    swal('Error');
    clearForm('update-picture-form');
}

function showFileLoader(callback) {
    swal({
        title: "Uploading...",
        showCancelButton: false,
        closeOnConfirm: false,
        showLoaderOnConfirm: true
    }, callback);
}

// Handle change EMAIL request.
function onChangeEmailClick() {
    $("#change-email-form").submit();
}

function onEmailChangeSuccess(response) {
    swal("Good job!", response.message, "success");
    clearForm('change-email-form');
}

function onEmailChangeFail(data) {
    swal(data.responseJSON.error);
    clearForm('change-email-form');
}

// Handle change PASSWORD request.
function onPasswordChangeSuccess(response) {
    swal("Good job!", response.message, "success");
    clearForm('change-password-form');
}

function onPasswordChangeFail(data) {
    swal(data.responseJSON.error);
    clearForm('change-password-form');
}

function clearForm(formId) {
    $(`#${formId}`)[0].reset();
}

// -- Team settings.
// Handle send join request.
function onInviteRequestSendSuccess(response) {
    swal("Good job!", response.message, "success");
}

function onInviteRequestSendFail(data) {
    swal(data.responseJSON.error);
}

function onProcessRequestSuccess(response) {
    swal(response.message, "", "success");
    $(`#user-${response.username}-request-form`).remove();
}

function onProcessRequestFail(data) {
    swal(data.error);
}

function onInviteRequestFail(data) {
    swal(data.responseJSON.error);
}

