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
 * 获取菜单集合
 */
function addMenu (data) {
  debugger
  return request({
    url: '/api/Base_SysNavigation/SaveData',
    method: 'post',
    data,
    headers: {
      'Content-Type': 'application/json'
    }
  })
}
export { GetMenuTrees, GetMenus, addMenu }
