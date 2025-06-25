$(document).ready(function () {
    UpdatePassword();
});

function UpdatePassword() {
    $('#edit-password').on('submit', function (event) {
        event.preventDefault(); // Ngăn không cho form load lại trang
        var formData = {
            password1: $('#password1').val(),
            password2: $('#password2').val()
        };

        $.ajax({
            url: '/Student/UpdatePassword',
            type: 'POST',
            data: formData, // Lấy tất cả dữ liệu trong form
            success: function (response) {
                $('#modal-content').html(response);
                // Tạo đối tượng modal và hiển thị
                var myModal = new bootstrap.Modal(document.getElementById('modal-content').querySelector('.modal'));
                myModal.show();
            }
        });
    });
}