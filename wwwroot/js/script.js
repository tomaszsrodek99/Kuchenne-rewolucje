
ClassicEditor
    .create(document.querySelector('.ingredients-editor'), {
        removePlugins: ['ImageUpload', 'ImageInsert', 'MediaEmbed'],
    })
    .then(ingredientsEditor => {
        window.ingredientsEditor = ingredientsEditor;
    })
    .catch(handleSampleError);


ClassicEditor
    .create(document.querySelector('.content-editor'), {

    })
    .then(contentEditor => {
        window.contentEditor = contentEditor;
    })
    .catch(handleSampleError);

function handleSampleError(error) {
    const issueUrl = 'https://github.com/ckeditor/ckeditor5/issues';

    const message = [
        'Oops, something went wrong!',
        `Please, report the following error on ${issueUrl} with the build id "mfywfzcq72aj-p4ttkmxaovz9" and the error stack trace:`
    ].join('\n');

    console.error(message);
    console.error(error);
}
