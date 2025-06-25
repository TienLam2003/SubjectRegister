
$(document).ready(function () {
    Delete();
    ShowModalDeleteConfirm();
    GetData();
    UpdateStatus();
});

function UpdateStatus() {
    $(document).on('click', '.updateStatus', function () {
        let code = $(this).data('code');
        let status = $(this).data('status');

        $.ajax({
            url: '/Semester/UpdateStatus',
            data: { code, status },
            type: 'post',
            success: function (response) {
                if (response.result) {
                    GetData();
                }
            }
        });
    });
}

function Delete() {
    $(document).on('click', '#linkDelete', function () {
        let code = $(this).data('id');
        let pageActive = $('.page-active').data('id');

        $.ajax({
            url: '/Semester/Delete',
            data: { code: code },
            type: 'post',
            cache: false,
            success: function (response) {
                if (response.result) {
                    GetData();
                }
                else {
                    $('#modal-content').html(response);
                    // Tạo đối tượng modal và hiển thị
                    var myModal = new bootstrap.Modal(document.getElementById('modal-content').querySelector('.modal'));
                    myModal.show();
                }
            }
        });
    });
}

function ShowModalDeleteConfirm() {
    $(document).on('click', '.showModalDeleteConfirm', function () {
        let code = $(this).data('code');
        $('#code').html(code);
        $('#linkDelete').data('id', code); // Gán data-id cho nút xóa
        $("#deleteConfirm").modal('show');
    });
}

function GetData() {
    $.ajax({
        url: '/Semester/GetAll',
        type: 'GET',
        success: function (data) {
            $('#content').html(data);
        }
    });
}
