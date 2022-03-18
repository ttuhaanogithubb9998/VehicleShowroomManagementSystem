
$(document).ready(() => {
    let loadFileE = (e) => {

        let outputImgEdit = document.getElementById('outputImgEdit');

        outputImgEdit.src = URL.createObjectURL(e.target.files[0]);

        outputImg.onload = () => {
            URL.revokeObjectURL(outputImg.src);
        }
    }

    let inputImgEdit = $('.avatarFileEdit');

    inputImgEdit.change(loadFileE)

})