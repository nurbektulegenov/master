$(document).ready(function () {
    $(function () {
        $('#pagination-bar').pagination({
            dataSource: function (done) {
                $.ajax({
                    type: 'post',
                    url: 'Home/GetBookData',
                    data: {  },
                    dataType: 'json',
                    success: function (response) {
                        console.log(response.data.Books);
                        console.log(response.data.PagesSize);
                        done(response.data.Books);
                    },
                    error: function () {
                        alert('Error!');
                    }
                });
            },
            pageSize: 20,
            ajax: {
                beforeSend: function () {
                    $('#pagination-data-container').html('Загрузка...');
                }
            },
            callback: function (data, pagination) {
                var html = template(data);
                $('#pagination-data-container').html(html);
            }
        });
    });
    function template(data) {
        var $table = $('<table/>').addClass('dataTable table table-striped');
        var $header = $('<thead/>').html('<tr><th>Книга</th><th>Автор</th><th>Isbn</th><th>Действия</th></tr>');
        $table.append($header);
        $.each(data, function (index, value) {
            var $row = $('<tr/>');
            var bookId = value.Id;
            $row.append($('<td/>').html(value.Name));
            $row.append($('<td/>').html(value.AuthorName));
            $row.append($('<td/>').html(value.Isbn));
            $row.append($('<td/>').html('<div data-id="' + bookId + '" class="actions">' +
                '<input type="submit" class="btn btn-outline-dark edit" value="Редактировать" />' +
                '<input type="button" class="btn btn-danger delete" value="Удалить" />' + '</div>'));
            $table.append($row);
        });
        $('#pagination-data-container').html($table);
        var bookId = $(".actions").data("id");
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