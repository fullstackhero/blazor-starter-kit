window.Download = (options) => {
    var url = "data:" + options.contentType + ";base64," + options.byteArray;
    var anchorElement = document.createElement("a");
    anchorElement.href = url;
    anchorElement.download = options.fileName;
    anchorElement.click();
    anchorElement.remove();
}