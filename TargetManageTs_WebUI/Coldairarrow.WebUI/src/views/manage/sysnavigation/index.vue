<template>
  <d2-container better-scroll>
    <template slot="header">

    </template>
    <el-container>
      <el-aside width="200px">
        <el-tree :data="treeData" :props="treeProps" @node-click="handleNodeClick" :expand-on-click-node="false"></el-tree>
      </el-aside>
      <el-main>
        <template>
          <div>
            <d2-crud
              :columns="tableColumns"
              :data="tableData"
              :loading="tableloading"
              :pagination="tablepagination"
              @pagination-current-change="paginationCurrentChange"/>
          </div>
        </template>
      </el-main>
    </el-container>
  </d2-container>
</template>

<script>
import { mapState, mapActions } from "vuex";
import { GetMenuTrees, GetMenus } from "@api/sys.navigation";
export default {
  title: "基础表格",
  index: 1,
  data() {
    return {
      treeData: [],
      treeProps: {
        children: "children",
        label: "label"
      },
      tableColumns: [
        {
          title: '时间',
          key: 'date',
          width: 320
        },
        {
          title: '姓名',
          key: 'name'
        },
        {
          title: '地址',
          key: 'address'
        }
      ],
      tableData: [],
      tableloading: false,
      tablepagination: {
        currentPage: 1,
        pageSize: 5,
        total: 0
      },
      selectNodeId:'', //选中的菜单Id
      condition:'', //查询字段名
      keyword:'', //查询的关键字

    };
  },
  computed: {
    ...mapState("d2admin/user", ["info"])
  },
  methods: {
    handleNodeClick(data) {
      this.selectNodeId=data.id;
      this.fetchData();
    },
    GetMenuTreesSelf() {
      var $this = this;
      GetMenuTrees()
        .then(async res => {
          $this.treeData = res.data;
        })
        .catch(err => {
          console.log("err: ", err);
          reject(err);
        });
    },
    paginationCurrentChange (currentPage) {
      this.tablepagination.currentPage = currentPage
      this.fetchData()
    },
    fetchData () {
      let $this=this;
      this.tableloading = true
      let [condition,keyword,pagination,id]=[this.condition,this.keyword,this.tablepagination,this.selectNodeId]
      GetMenus({condition,keyword, pagination,id}).then(res => {
        $this.tableData = res.list
        $this.tablepagination.total = res.page.total
        $this.tableloading = false
      }).catch(err => {
        console.log('err', err)
        this.tableloading = false
      })
    }
  },
  created:function(){
    this.GetMenuTreesSelf();
  }
};
</script>

<style>
.el-container {
  height: 500px;
  border: 1px solid #eee;
}

.el-header {
  background-color: #b3c0d1;
  color: #333;
  line-height: 60px;
}

.el-aside {
  border-right: 1px solid #eee;
}
</style>