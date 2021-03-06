import request from '@/plugin/axios'

/**
 * 获取菜单树
 */
function GetMenuTrees () {
  return request({
    url: '/api/Base_SysNavigation/GetMenuTrees',
    method: 'get',
    headers: {
      'Content-Type': 'application/json'
    }
  })
}
/**
 * 获取菜单集合
 */
function GetMenus (data) {
  return request({
    url: '/api/Base_SysNavigation/GetDataList',
    method: 'post',
    data,
    headers: {
      'Content-Type': 'application/json'
    }
  })
}

/**
 * 新增菜单
 */
function addMenu (data) {
  return request({
    url: '/api/Base_SysNavigation/SaveData',
    method: 'post',
    data,
    headers: {
      'Content-Type': 'application/json'
    }
  })
}

/**
 * 编辑菜单
 */
function updateMenu (data) {
  return request({
    url: '/api/Base_SysNavigation/SaveData',
    method: 'post',
    data,
    headers: {
      'Content-Type': 'application/json'
    }
  })
}

/**
 * 删除菜单
 */
function deleteMenu (data) {
  return request({
    url: '/api/Base_SysNavigation/DeleteData?ids=' + data.ids,
    method: 'get',
    headers: {
      'Content-Type': 'application/json'
    }
  })
}

export { GetMenuTrees, GetMenus, addMenu, updateMenu, deleteMenu }
