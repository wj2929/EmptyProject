$(function() {

    $("input[type=text]").addClass("form_text");
    $("textarea").addClass("form_textarea");
    $("table.table_edit>tbody>tr:odd").addClass("int");
    $("table.list_table>tbody>tr,table.list_table_i>tbody>tr").live("mouseover", function() {
        $(this).addClass("on");
    });
    $("table.list_table>tbody>tr,table.list_table_i>tbody>tr").live("mouseout", function() {
        $(this).removeClass("on");
    });

})
