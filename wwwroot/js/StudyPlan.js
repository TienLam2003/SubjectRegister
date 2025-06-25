
GetData();
$(document).ready(function () {
    Delete();
    ShowModalDeleteConfirm();
});

function Delete() {
    $(document).on('click', '#linkDelete', function () {
        let code = $(this).data('id');
        let pageActive = $('.page-active').data('id');

        $.ajax({
            url: '/StudyPlan/Delete',
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

//Show modal xác nhận xóa
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
        url: '/StudyPlan/GetByStudent',
        type: 'GET',
        cache: false,
        success: function (data) {
            $('#content').html(data);
        }
    });
}