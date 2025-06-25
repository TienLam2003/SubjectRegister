
GetData(1);
$(document).ready(function () {
    Search();
    Pagination();
    Delete();
    ShowModalDeleteConfirm();
});

function Delete() {
    $(document).on('click', '#linkDelete', function () {
        let code = $(this).data('id');
        let pageActive = $('.page-active').data('id');

        $.ajax({
            url: '/Subject/Delete',
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

//Show modal xác nhận xóa môn học trong bảng
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
        let subjectCode = $(this).data('code');
        let subjectName = $(this).data('name');
        if (subjectCode == null || subjectCode == "" && subjectName == null || subjectName == "") {
            GetData(pageActive);
        }
        else {
            let dataFrom = {
                SubjectCode: subjectCode,
                SubjectName: subjectName,
                PageActive: pageActive
            };
            GetBySearch(dataFrom);
        }
    });
}

function GetData(pageActive) {
    $.ajax({
        url: '/Subject/GetPagination',
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
        let subjectCode = $("#SubjectCode").val();
        let subjectName = $("#SubjectName").val();
        if (subjectCode == null && subjectName == null) return;

        //Object
        let dataFrom = {
            SubjectCode: subjectCode,
            SubjectName: subjectName,
            PageActive: 1
        };

        GetBySearch(dataFrom);
    });
}

function GetBySearch(dataFrom) {
    $.ajax({
        url: '/Subject/GetBySearch',
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
