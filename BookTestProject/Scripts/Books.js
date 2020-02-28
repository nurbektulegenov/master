$(document).ready(function () {
    loadData();

    function loadData() {
        $.ajax({
            url: 'Home/GetBookData',
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                var $table = $('<table/>').addClass('dataTable table table-striped');
                var $header = $('<thead/>').html('<tr><th>Книга</th><th>Автор</th><th>Isbn</th><th>Действия</th></tr>');
                $table.append($header);
                $.each(data, function (i, value) {
                    $.each(value, function (index, val) {
                        var $row = $('<tr/>');
                        var bookId = val.Id;
                            $row.append($('<td/>').html(val.Name));
                            $row.append($('<td/>').html(val.AuthorName));
                            $row.append($('<td/>').html(val.Isbn));
                        $row.append($('<td/>').html('<div data-id="' + bookId + '" class="actions">' +
                            '<input type="submit" class="btn btn-outline-dark edit" value="Редактировать" />' +
                            '<input type="button" class="btn btn-danger delete" value="Удалить" />' +
                            '</div>'));
                        $table.append($row);
                    });
                }); 
                $('#updatePanel').html($table);
                var bookId = $(".actions").data("id");
                editButtonClick(bookId);
                deleteButtonClick(bookId);
            },
            error: function()  {
                alert('Error!');
            }
        });
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

    //редактирование можно так открыть, но там верстку модального окна надо доделать
    //function editButtonClick(id) {
    //    $(".edit").click(function() {
    //        $.ajax({
    //            type: "get",
    //            url: "/Home/Edit",
    //            data: { id: id },
    //            success: function(response) {
    //                $("#dialog").html(response);
    //                $("#dialog").dialog("open");
    //            },
    //            failure: function(response) {
    //                alert(response.responseText);
    //            },
    //            error: function(response) {
    //                alert(response.responseText);
    //            }
    //        });
    //    });
    //}
});