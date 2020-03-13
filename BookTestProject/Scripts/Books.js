$(document).ready(function () {
    $(document).ajaxStart(function(){
        $("#loading").show();
    }).ajaxStop(function() {
        $("#loading").hide();
    });
    $(function () {
        $('#pagination-bar').pagination({
            dataSource: function (done) {
                $.ajax({
                    type: 'post',
                    url: 'Home/GetBookData',
                    data: {  },
                    dataType: 'json',
                    success: function (response) {
                        done(response.data.Books);
                    },
                    error: function () {
                        alert('Error!');
                    }
                });
            },
            pageSize: 10,
            ajax: {
                beforeSend: function () {
                    $('#pagination-data-container').html('Загрузка....');
                }
            },
            callback: function (data, pagination) {
                console.log(data);
                let html = template(data);
                $('#pagination-data-container').html(html);
            }
        });
    });
    function template(data) {
        let $table = $('.tableBody');
        $("tr:has(td)").remove();
        $.each(data, function (index, value) {
            let $row = $('<tr/>');
            $row.append($('<td/>').text(value.Name));
            $row.append($('<td/>').text(value.AuthorName));
            $row.append($('<td/>').text(value.Isbn));
            $row.append($('<td/>').html('<div data-id="' + value.Id + '" class="actions">' +
                '<input type="submit" class="btn btn-outline-dark edit" value="Редактировать" />' +
                '<input type="button" class="btn btn-danger delete" value="Удалить" />' + '</div>'));
            $table.append($row);
        });
        let bookId = $(".actions").data("id");
        editButtonClick(bookId);
        deleteButtonClick(bookId);
    }

    function deleteButtonClick(id) {
        $(".delete").click(function() {
            $.ajax({
                type: "post",
                data: { id: id },
                url: "/Home/Delete",
                success: function() {
                    alert("Книга удалена");
                    location.reload();
                },
                failure: function(response) {
                    alert(response.responseText);
                },
                error: function(response) {
                    alert(response.responseText);
                }
            });
        });
    }

    function editButtonClick(id) {
        $(".edit").click(function () {
            window.location.href = "/Home/Edit/" + id;
        });
    }
});