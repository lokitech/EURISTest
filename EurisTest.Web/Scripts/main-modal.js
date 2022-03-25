$(function () {
    $('.open-modal').click(function () {
        $('#modal-content').load(this.href);
        $("#modal").modal();
        return false;
    });
});