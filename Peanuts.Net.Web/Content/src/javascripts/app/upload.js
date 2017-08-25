function FileUploader(selector, optionsParam) {

    var INDEX_PLACEHOLDER = "___FILEUPLOADER_INDEX_PLACEHOLDER___";
    
    var options = {
        urlUpload: optionsParam.urlUpload,
        urlCurrent: optionsParam.urlCurrent,
        urlFallback: optionsParam.urlFallback,

        clickOrDropHint: optionsParam.clickOrDropHint || 'Click or Drop image here',
        imageAlt: optionsParam.imageAlt || 'Image',
        imageTitle: optionsParam.imageTitle || 'Image',

        isMultipleFileUpload: optionsParam.isMultipleFileUpload || false
    }

    function browse() {
        var evt = document.createEvent("MouseEvents");
        evt.initEvent("click", true, false);
        uploadInput.get()[0].dispatchEvent(evt);
    }

    function upload(files) {

        if (!files) {
            return;
        }

        postNewOrEmptyFiles(files);

        /*clear input that files are not uploaded on post*/
        uploadInput.val(null);
    }

    function removeFile(fileToRemove) {
        uploadedFiles = jQuery.grep(uploadedFiles, function (file) {
            return file != fileToRemove;
        })

        fileToRemove.removeFile();
    }


    function postNewOrEmptyFiles(filesToUpload) {

        if (!filesToUpload || filesToUpload.length < 1) {
            /*Only post if files included*/
            return;
        }

        if (options.isMultipleFileUpload != true) {
            /* If only single upload available, use only the first file*/
            filesToUpload = [filesToUpload[0]];
            $(uploadedFiles).each(function (index, uploadedFile) {
                removeFile(uploadedFile);
            });
        }

        $(filesToUpload).each(function (index, file) {

            /* Template mit eingefügtem Dateinamen */
            var newElement;
            var indexValue = 'index_value_' + Date.now();
            if (options.isMultipleFileUpload != true) {
                newElement = $(newFileTemplate);
            } else {
                newElement = $(newFileTemplate.split(INDEX_PLACEHOLDER).join(indexValue));
            }
            
            newElement.find(".uploaded-file-name").text(file.name);

            filesContainer.append(newElement);
            var newFile = new UploadedFile(newElement[0]);
            uploadedFiles[uploadedFiles.length] = newFile;

            newFile.isUploading(true);
            newFile.setUploadProgress(0);

            $(newFile).on("FileUploader:DeleteFile", function (e, file) {
                removeFile(file);
            });

            //XMLHttpRequest Objekt erzeugen
            var request = new XMLHttpRequest();

            var formData = new FormData();
            if (file) {
                formData.append("file.FileInfo", file);
                formData.append("file.FileName", file.name);

                if (options.isMultipleFileUpload != true) {
                    formData.append("path", uploadInputName)
                } else {
                    formData.append("path", uploadInputName + "[" + (indexValue) + "]")
                }
            }

            function onError(e) {

            }
            function onComplete(e) {
                if (this.status == 200) {
                    onSuccess(e, this);
                } else {
                    onError(e, this);
                }
            }

            function onSuccess(e, xhr) {
                newFile.isUploading(false);
                newFile.$content.find(".js-fileuploader-uploaded-file-content").html(xhr.response);
                console.log("File uploaded: " + newFile.filename);
            } 

            function onError(e, xhr) {
                newFile.isUploading(false);
                console.log("Upload Error: " + newFile.filename);
                removeFile(newFile);
                newFile.error();
                newFile.removeFile();
            }

            function onCancel(e) {
                newFile.isUploading(false);
                console.log("Upload Error: " + newFile.filename);
                removeFile(newFile);
                newFile.removeFile();
            }

            function onProgress(e) {
                if (e.lengthComputable) {
                    var percentComplete = Math.round(e.loaded * 100 / e.total);
                    newFile.setUploadProgress(percentComplete);
                    console.log("Progress for " + newFile.filename + ": " + percentComplete);
                } else {
                    newFile.setUploadProgress(10);
                }
            }

            request.upload.onprogress = function (e) {
                var p = Math.round(100 / e.total * e.loaded);
                newFile.setUploadProgress(p);
                console.log("Progress for " + newFile.filename + ": " + p);
            };

            request.addEventListener("load", onComplete, false);
            request.addEventListener("error", onError, false);
            request.addEventListener("abort", onCancel, false);

            request.open("POST", options.urlUpload);
            request.send(formData);
        });

    }

    function deletePreview() {
        uploadInput.val("");
        postNewOrEmptyFile(null);
    }

    function registerDragAndDrop() {

        uploadControl.on('dragenter', function (e) {
            e.stopPropagation();
            e.preventDefault();
            uploadControl.addClass('can-drop');
        });
        uploadControl.on('dragleave', function (e) {
            e.stopPropagation();
            e.preventDefault();
            uploadControl.removeClass('can-drop');
        });
        uploadControl.on('dragover', function (e) {
            e.stopPropagation();
            e.preventDefault();
            uploadControl.addClass('can-drop');
        });
        uploadControl.on('drop', function (e) {
            uploadControl.removeClass('can-drop');
            e.preventDefault();
            var files = e.originalEvent.dataTransfer.files;
            if (files.length > 0) {
                upload(files);
            }
        });

    }

    function registerEvents() {
        $(browseFilesAnchor).click(function (e) {
            console.log("Upload-Control clicked");
            e.preventDefault();
            e.stopPropagation();
            browse();

        });

        uploadInput.change(function () {
            var that = this;
            upload(that.files);
        });
    }

    var uploadControl = $(selector);
    var uploadInput = $(selector).find(".js-fileuploader-input");
    var browseFilesAnchor = $(selector).find(".js-fileuploader-browse");
    var filesContainer = uploadControl.find(".js-fileuploader-uploaded-files-container");
    var uploadInputName = $(uploadInput).attr("name");
    $(uploadInput).removeAttr("name")

    var newFileTemplate = $("<div/>").html(uploadControl.find("template.new-file-template").html()).html();
    var uploadedFiles = [];
    
    filesContainer.children(".js-fileuploader-uploaded-file").each(function (index, image) {
        uploadedFiles[index] = new UploadedFile(image);
    });

    registerEvents();
    registerDragAndDrop();

    if (options.urlCurrent) {
        refreshPreview();
    }

    return {
        browse: browse,
        uploadFile: upload,
        options: options,

    };
};


function UploadedFile(uploadedFile) {

    console.log("Neue Datei soll hochgeladen werden.")

    var that = $(uploadedFile);
    var self = this;
    var _filename = $(uploadedFile).find(".uploaded-file-name").text();
    var progressBar = $(uploadedFile).find(".js-fileuploader-progress-bar");
    var previewImage = $(uploadedFile).find(".js-fileupload-uploaded-file-preview");
    var $content = $(uploadedFile);

    /*
     * Sets or gets the upload-state of the File.
     * isUploading(true) adds the class "uploading".
     * isUploading(false) removes the class "uploading".
     * isUploading() gets wether the file has (true) the class "uploading" or not (false).
     */
    function isUploading(uploading) {
        if (typeof uploading === "undefined") {
            return that.hasClass("uploading");
        }

        if (uploading == true) {
            that.addClass("uploading");
            previewImage.css('backgroundImage', 'none');
        } else {
            that.removeClass("uploading");
        }
    }

    function onRemove() {
        that.fadeOut(function (e) {
            that.remove();
        });

    }

    function onError() {
        that.addClass("error");
    }

    /*
     * Sets the progress of the upload-progress-bar.
     * The File is set to isUploading(true).
     */
    function setUploadProgress(progress) {
        console.log("Upload-progress: " + progress);
        var newWidth = progress + "%";
        console.log("Neue Breite concat: " + newWidth);
        progressBar.css('width', newWidth);
        console.log("Neue Breite: " + $(progressBar).width());
    }

    function setPreviewImage(src) {
        previewImage.style.backgroundImage = "url('" + src + "')";
    }

    function registerEvents() {
        that.on("click", ".js-fileupload-delete", function (e) {
            console.log("Delete-Button clicked");
            e.stopPropagation();
            e.preventDefault();
            onRemove();
            that.trigger("FileUploader:DeleteFile", self);
        });

        $(previewImage).on("error", function (e) {
            if (!that.hasClass("fallback")) {
                that.addClass("fallback");
                if (options.urlFallback) {
                    previewImage.style.backgroundImage(options.urlFallback);
                    return;
                }
            }
            that.addClass("no-image");
            previewImage.style.backgroundImage = "none";
            e.preventDefault();
        });
    }

    registerEvents();

    return {
        filename: _filename,
        isUploading: isUploading,
        setUploadProgress: setUploadProgress,
        setPreviewImage: setPreviewImage,
        removeFile: onRemove,
        error: onError,
        $content: $content
    }
}


$ = jQuery;
$.fn.extend({
    fileUploader: function (optionsParam) {
        return this.each(function (index, item) {
            console.log("Start Initializing file-upload " + index + " Item: " + item);

            var optionsUrlUpload = null;
            var optionsUrlCurrent = null;
            var optionsUrlFallback = null;
            var optionsClickOrDropHint = null;

            if (optionsParam) {
                optionsUrlUpload = optionsParam.urlUpload || null;
                optionsUrlCurrent = optionsParam.urlCurrent || null;
                optionsUrlFallback = optionsParam.urlFallback || null;
                optionsClickOrDropHint = optionsParam.clickOrDropHint || null;
            }

            var options = {
                urlUpload: $(item).data("url-upload") || optionsUrlUpload,
                urlCurrent: $(item).data("url-current") || optionsUrlCurrent,
                urlFallback: $(item).data("url-fallback") || optionsUrlFallback,
                clickOrDropHint: optionsClickOrDropHint,
                isMultipleFileUpload: ($(item).data("is-multiple-upload") == "True" || $(item).data("is-multiple-upload") == "true" || $(item).data("is-multiple-upload") == true),
            }
            var uploadControl = new FileUploader(item, options);
            console.log("Finish Initializing file-upload " + index);
        });
    }
});

$(function () {
    console.log("Initializing file-uploads");
    $(".js-fileuploader").fileUploader();
});
