function PreviewImage() {
    var oFReader = new FileReader();
    oFReader.readAsDataURL(document.getElementById("uploadImage").files[0]);

    oFReader.onload = function (oFREvent) {
        document.getElementById("uploadPreview").style.display = 'visible';
        document.getElementById("uploadPreview").style.width = 100 + 'px';
        document.getElementById("uploadPreview").style.height = 100 + 'px';
        document.getElementById("uploadPreview").src = oFREvent.target.result;
        document.getElementById("uploadPreviewOld").style.display = 'none';
    };
};

