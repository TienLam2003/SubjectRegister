
GetData(1);
$(document).ready(function () {
    Search();
    Pagination();
    ShowModalAvatarUser();
    Delete();
    ShowModalDeleteConfirm();
});

function Delete() {
    $(document).on('click', '#linkDelete', function () {
        let code = $(this).data('id');
        let pageActive = $('.page-active').data('id');

        $.ajax({
            url: '/Teacher/Delete',
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

//Show modal chứa ảnh user trong bảng
function ShowModalAvatarUser() {
    $(document).on('click', '.show-modal', function () {
        let dataImg = $(this).data("img");
        $("#img-user").attr("src", dataImg);
        $("#editAvatarModal").modal('show');
    });
}

//Show modal xác nhận xóa user trong bảng
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
        let code = $(this).data('code');
        let fullname = $(this).data('fullname');
        if (code == null || code == "" && fullname == null || fullname == "") {
            GetData(pageActive);
        }
        else {
            let dataFrom = {
                Teacher: code,
                Fullname: fullname,
                PageActive: pageActive
            };
            GetBySearch(dataFrom);
        }
    });
}

function GetData(pageActive) {
    $.ajax({
        url: '/Teacher/GetPagination',
        data: { pageActive: pageActive },
        type: 'GET',
        cache: false,
        success: function (data) {
            $('#content').html(data);
        }
    });
}

function Search() {
    $(document).on('click', '#search', function () {
        let teacherCode = $("#TeacherCode").val();
        let fullname = $("#Fullname").val();
        if (teacherCode == null && fullname == null) return;

        //Object
        let dataFrom = {
            TeacherCode: teacherCode,
            Fullname: fullname,
            PageActive: 1
        };

        GetBySearch(dataFrom);
    });
}

function GetBySearch(dataFrom) {
    $.ajax({
        url: '/Teacher/GetBySearch',
        type: 'GET',
        data: dataFrom,
        cache: false,
        beforeSend: function () {
            $('.table tbody').empty();
            $('.card').after(`<div id="loading">
                        <img id="loading-icon" src="/images/Load.gif" alt="Loading..."></div>`);
        },
        success: function (data) {
            $('#loading').remove();
            $('#content').html(data);
        },
        complete: function () {
            /*alert(2);*/
        },
        error: function () {

        }
    });
}
