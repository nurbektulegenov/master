$(document).ready(function() {
    function template(data) {
        var html = '<ul>';
        $.each(data, function (index, item) {
            html += '<li>' + item + '</li>';
        });
        html += '</ul>';
        return html;
    }

    $(function () {
        var container = $('#pagination-bar');
        container.pagination({
            dataSource: loadData(1),
            pageSize: 20,
            callback: function (data, pagination) {
                var html = template(data);
                $('#pagination-data-container').html(html);
            }
        });
    });

    function loadData(pageIndex) {
        $.ajax({
            type: 'post',
            url: 'Home/GetBookData',
            data: { pageIndex: pageIndex },
            dataType: 'json',
            success: onSuccess(),
            error: function() {
                alert('Error!');
            }
        });
    }

    function onSuccess(response) {
        var $table = $('<table/>').addClass('dataTable table table-striped');
        var $header = $('<thead/>').html('<tr><th>Книга</th><th>Автор</th><th>Isbn</th><th>Действия</th></tr>');
        $table.append($header);
        $.each(response, function(i, value) {
            $.each(value, function (index, val) {
                var $row = $('<tr/>');
                var bookId = val.Id;
                $row.append($('<td/>').html(val.Name));
                $row.append($('<td/>').html(val.AuthorName));
                $row.append($('<td/>').html(val.Isbn));
                $row.append($('<td/>').html('<div data-id="' + bookId + '" class="actions">' +
                            '<input type="submit" class="btn btn-outline-dark edit" value="Редактировать" />' +
                            '<input type="button" class="btn btn-danger delete" value="Удалить" />' + '</div>'));
                $table.append($row);
                $.count = val.PagesSize;
            });
        });
        $('#updatePanel').html($table);
        var bookId = $(".actions").data("id");
        editButtonClick(bookId);
        deleteButtonClick(bookId);
        return response;
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