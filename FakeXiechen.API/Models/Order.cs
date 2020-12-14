using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Stateless;

namespace FakeXiechen.API.Models
{
    public enum OrderStateEnum
    {
        Pending,        //订单生成
        Processing,     //支付处理中
        Completed,      //订单完成
        Cancelled,      //订单取消
        Declined,       //交易失败
        Refund,         //订单退款
    }

    public enum OrderStateTriggerEnum
    {
        PlaceOrder, //下单支付
        Approve,    //支付成功
        Reject,     //支付失败
        Cancel,     //取消
        Return,     //退货
    }
    public class Order
    {
        public Order()
        {
            StateMachineInit();
        }

        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }  //外键
        public AppUser User { get; set; }   //反向导航
        public ICollection<LineItem> ShoppingCartItems { get; set; }
            = new List<LineItem>();

        public OrderStateEnum State { get; set; }
        public DateTime CreateDateUTC { get; set; }
        public string TranscationMetadata { get; set; }

        private StateMachine<OrderStateEnum, OrderStateTriggerEnum> _machine { get; set; }

        private void StateMachineInit()
        {
            _machine = new StateMachine<OrderStateEnum, OrderStateTriggerEnum>(OrderStateEnum.Pending); // 创建并初始化状态机初始状态

            _machine.Configure(OrderStateEnum.Pending)                               //配置当前状态(事件)在触发特定动作后将会迁移到那个状态
                .Permit(OrderStateTriggerEnum.PlaceOrder, OrderStateEnum.Processing) //触发指定动作->进入指定状态(对应的事件)
                .Permit(OrderStateTriggerEnum.Cancel, OrderStateEnum.Cancelled);

            _machine.Configure(OrderStateEnum.Processing)
                .Permit(OrderStateTriggerEnum.Approve, OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Reject, OrderStateEnum.Declined);

            _machine.Configure(OrderStateEnum.Completed)
                .Permit(OrderStateTriggerEnum.Return, OrderStateEnum.Refund);
        }

    }
}
