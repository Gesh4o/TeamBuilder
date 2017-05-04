window.onload = function () {
    document.getElementById('update-picture-form').onsubmit = function () {
        let formdata = new FormData(); //FormData object
        let fileInput = document.getElementById('file-input');

        //Iterating through each files selected in fileInput
        for (let i = 0; i < fileInput.files.length; i++) {
            //Appending each file to FormData object
            formdata.append(fileInput.files[i].name, fileInput.files[i]);
        }

        //Creating an XMLHttpRequest and sending
        let xhr = new XMLHttpRequest();
        function onReady () {
            if (xhr.readyState === 4 && xhr.status === 200) {
                let data = xhr.responseText;
                onProfilePictureUpdateSuccess(data);
            } else if (xhr.readyState === 4 && xhr.status !== 200) {
                let data = xhr.responseText;
                onProfilePictureUpdateFail(data);
            }
        }

        xhr.onreadystatechange = onReady;
        xhr.open('POST', '/Manage/ChangeProfilePicture');
        xhr.send(formdata);
        showFileLoader(onReady);
        return false;
    }
}