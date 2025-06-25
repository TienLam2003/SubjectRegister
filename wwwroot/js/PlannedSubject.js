
GetData(1);
$(document).ready(function () {
    Pagination();
    Delete();
    ShowModalDeleteConfirm();
});

function Delete() {
    $(document).on('click', '#linkDelete', function () {
        let code = $(this).data('id');
        let pageActive = $('.page-active').data('id');

        $.ajax({
            url: '/PlannedSubject/Delete',
            data: { code: code },
            type: 'post',
            cache: false,
            success: function (response) {
                if (response.result) {
                    GetData(pageActive);
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

function Pagination() {
    $(document).on('click', '.page-number', function () {
        let pageActive = $(this).data('id');
        GetData(pageActive);
    });
}

function GetData(pageActive) {
    $.ajax({
        url: '/PlannedSubject/GetPagination',
        data: { pageActive: pageActive },
        type: 'GET',
        cache: false,
        success: function (data) {
            $('#content').html(data);
        }
    });
}