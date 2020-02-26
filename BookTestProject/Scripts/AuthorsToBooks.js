$(document).ready(function () {
    $(".delete").click(function (event) {
        var userName = $(this).data("id");
        console.log(userName);
        $.ajax({
            type: "get",
            data: { name: userName},
            url: "/Author/BooksToAuthors",
            success: function(response) {
                //if (response.INT <= 0)
                var confResult = confirm("У автора есть " + response + " книга(и), продолжить удаление?");
                if (confResult) {
                    $.ajax({
                        type: "post",
                        data: { UserName: userName },
                        url: "/Author/Delete",
                        success: function() {
                            alert("Автор удален");
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
