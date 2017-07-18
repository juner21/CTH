

//测试数据
var testData = {
    test: [{
        Id:'1',
        Name: '用户管理',
        List: [{ Title: '主页1', Href: '#foo' }, { Title: '主页2', Href: '#bar' }, { Title: '主页3', Href: '/User/M3' }],
        Show:false,
    }, {
        Id: '2',
        Name: '订单',
        List: [{ Title: '订单1', Href: '' }, { Title: '订单2', Href: '' }, { Title: '订单3', Href: '' }],
        Show: false,
    }]
};

//菜单 组件注册
Vue.component('menulist', {
    props: ['md'],
    template: `<div>
                    <ul>
                        <li v-for="item in md"  class ="mlist">
                            <a v-on:click="showList(item.Id)" :class="{noActity:true,actity:item.Show}">{{item.Name}}</a>
                            <ul v-for="obj in item.List" v-if="item.Show">
                                <li><a v-bind:href='obj.Href' class ="noActity">{{obj.Title}}</a></li>
                            </ul>
                        </li>
                    </ul>
                </div>
              `,
    methods: {
        showList: function (id) {
            var list = this._props.md;
            for (var i = 0; i < list.length; i++) {
                if (list[i].Id == id)
                    list[i].Show = true;
                else
                    list[i].Show = false;
            }
        }
    }
})

//路由配置

require.config(
    {
        paths: {
            User: "../JS/Menu/User/User",
        }
    }
);

require(["User"], function (u) {

    const routes = [
        {
            path: '/foo', component: u.User
        },
        { path: '/bar', component: u.Login }
    ]

    const router = new VueRouter({
        routes
        });


    new Vue({
            el: '#app',
            data: testData,
            router
        });
});


























