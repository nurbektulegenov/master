$(document).ready(function () {
    $(".delete").click(function () {
        var userName = $(this).data("name");
        $.ajax({
            type: "get",
            data: { name: userName},
            url: "/Author/BooksToAuthors",
            success: function(response) {
                var confResult = confirm("У автора есть " + response + " книга(и), продолжить удаление?");
                if (confResult) {
                    $.ajax({
                        type: "post",
                        data: { UserName: userName },
                        url: "/Author/Delete",
                        success: function (data) {
                            alert("Автор удален");
                            location.reload();
                            
                        },
                        failure: function(response) {
                            alert(response.responseText);
                        },
                        error: function(response) {
                            alert(response.responseText);
                        }
                    });
                }
            },
            failure: function(response) {
                alert(response.responseText);
            },
            error: function(response) {
                alert(response.responseText);
            }
        });
    });
});
