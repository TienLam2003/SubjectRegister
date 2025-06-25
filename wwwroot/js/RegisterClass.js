
GetData();
$(document).ready(function () {
    ShowModalDeleteConfirm();
    Delete();
});

function ShowModalDeleteConfirm() {
    $(document).on('click', '.showModalDeleteConfirm', function () {
        let code = $(this).data('code');
        $('#linkDelete').attr('data-id', code); // Gán data-id cho nút xóa
        $("#deleteConfirm").modal('show');
    });
}

function Delete() {
    $(document).on('click', '#linkDelete', function () {
        let code = $(this).attr('data-id');
        console.log(code);
        $.ajax({
            url: '/RegisterClass/Delete',
            data: { code : code },
            type: 'post',
            cache: false,
            success: function (response) {
                if (response.result) {
                    GetData();
                }
            }
        });
    });
}

function GetData() {
    $.ajax({
        url: '/RegisterClass/GetScheduleByStudent',
        type: 'GET',
        cache: false,
        success: function (data) {
            $('#content').html(data);
        }
    });
}