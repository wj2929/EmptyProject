var taskPlanMessageHub;
$(function () {
    taskPlanMessageHub = $.connection.taskPlanMessageHub;

    taskPlanMessageHub.client.sendTaskMessage = function (message) {
        alert(message);
    };

    $.connection.hub.start()
        .done(function () { })
        .fail(function () {
            alert("与服务器连接失败，部分服务将受到影响，点击确定刷新页面重建建立连接！");
            location.href = location.href;
        });
    });

function ImmediatelyStartTaskPlan(Id){
    taskPlanMessageHub.server.immediatelyStartTaskPlan(Id);
}