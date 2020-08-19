/**
 * Created by 歌 on 2017/7/1.
 */
declare var $;
declare var bootbox;
declare var swal;
export function Error(Message: string) {
  bootbox.alert(Message);
}

export function SweetError(Title: string, Message: string) {
  swal(Title, Message, "error");
}


export function SweetSuccess(Title: string, Message: string) {
  swal(Title, Message, "success");
}


export function SweetConfirm(Title: string, Message: string, callback: (isConfirm: boolean) => void) {
  swal({
    title: Title,
    text: Message,
    type: "warning",
    showCancelButton: true,
    confirmButtonColor: "#DD6B55",
    confirmButtonText: "确定",
    cancelButtonText: "取消",
    closeOnConfirm: true,
    closeOnCancel: true
  },
    function (isConfirm) {
      callback(isConfirm);
    });
}

export function SweetClose(){
  swal.close();
}


export function Success(Message: string) {
  bootbox.alert(Message);
}


export function ShowBlockUI(blockId?: string) {
  if (blockId) {
    blockId = blockId.replace("#", "");
    $(`#${blockId}`).block({
      message: '<div class="blockui-message" style="display:block"><i class="icon-spinner10 spinner"></i><span class="text-semibold display-block">处理中...</span></div>',
      overlayCSS: {
        backgroundColor: '#fff',
        opacity: 0.8,
        cursor: 'wait'
      },
      css: {
        width: 100,
        '-webkit-border-radius': 2,
        '-moz-border-radius': 2,
        border: 0,
        padding: 0,
        backgroundColor: 'transparent'
      }
    });
  }
  else
    $.blockUI({
      message: '<div class="blockui-message" style="display:block"><i class="icon-spinner10 spinner"></i><span class="text-semibold display-block">处理中...</span></div>',
      overlayCSS: {
        backgroundColor: '#fff',
        opacity: 0.8,
        cursor: 'wait'
      },
      css: {
        width: 100,
        '-webkit-border-radius': 2,
        '-moz-border-radius': 2,
        border: 0,
        padding: 0,
        backgroundColor: 'transparent'
      }
    });
}

export function HideBlockUI(blockId?: string) {
  if (blockId) {
    blockId = blockId.replace("#", "");
    $(`#${blockId}`).unblock();
  }
  else
    $.unblockUI();
}

