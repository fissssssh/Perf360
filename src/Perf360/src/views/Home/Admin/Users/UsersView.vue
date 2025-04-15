<template>
  <el-button class="mb-4" type="primary" @click="addUserDialogVisible = true">Add User</el-button>
  <el-table :data="users" stripe>
    <el-table-column label="Name" prop="nickname" />
    <el-table-column label="Email" prop="email" />
    <el-table-column label="Phone Number" prop="phoneNumber" />
    <el-table-column label="Roles" prop="roles">
      <template #default="scope">
        <div class="flex flex-wrap space-x-2 space-y-2">
          <el-tag v-for="role in scope.row.roles" :key="role">{{ role }} </el-tag>
        </div>
      </template>
    </el-table-column>
    <el-table-column>
      <template #default="scope">
        <el-button :icon="UserIcon" @click="openEditUserRolesDialog(scope.row.id)"></el-button>
        <el-popconfirm
          :hide-after="0"
          :title="`Are you sure to delete User '${scope.row.name}'`"
          @confirm="deleteUser(scope.row.id)"
        >
          <template #reference>
            <el-button type="danger" :icon="Delete"></el-button>
          </template>
        </el-popconfirm>
      </template>
    </el-table-column>
  </el-table>

  <el-dialog v-model="addUserDialogVisible" title="Add User">
    <el-form v-model="addUserForm" @submit.prevent>
      <el-form-item label="UserName">
        <el-input v-model="addUserForm.username" />
      </el-form-item>
      <el-form-item label="NickName">
        <el-input v-model="addUserForm.nickname" />
      </el-form-item>
      <el-form-item label="Email">
        <el-input v-model="addUserForm.email"></el-input>
      </el-form-item>
      <el-form-item label="Phone Number">
        <el-input v-model="addUserForm.phoneNumber"></el-input>
      </el-form-item>
    </el-form>
    <template #footer>
      <el-button @click="addUserDialogVisible = false">Cancel</el-button>
      <el-button type="primary" @click="createUser">Confirm</el-button>
    </template>
  </el-dialog>

  <el-dialog v-model="editUserRolesDialogVisible" title="Edit User's Roles">
    <el-checkbox-group v-model="userRoles">
      <el-checkbox
        v-for="role in roles"
        :key="role.id"
        :label="role.name"
        :value="role.name"
      ></el-checkbox>
    </el-checkbox-group>
    <template #footer>
      <el-button @click="editUserRolesDialogVisible = false">Cancel</el-button>
      <el-button type="primary" @click="editUserRoles()">Confirm</el-button>
    </template>
  </el-dialog>
</template>

<script setup lang="ts">
import api from '@/api'
import type { Role } from '@/models/role'
import type { AddUser, User } from '@/models/user'
import { Delete, User as UserIcon } from '@element-plus/icons-vue'
import { onMounted, type Ref } from 'vue'

const users: Ref<User[]> = ref([])
const addUserDialogVisible = ref(false)
const addUserForm: Ref<AddUser> = ref({})
const editUserRolesDialogVisible = ref(false)
const roles: Ref<Role[]> = ref([])
const userId: Ref<string> = ref('')
const userRoles: Ref<string[]> = ref([])

onMounted(() => {
  getUsers()
})

async function getUsers() {
  users.value = await api.users.getUsers()
}

async function createUser() {
  try {
    await api.users.createUser(addUserForm.value)
    await getUsers()
  } catch (error) {
    console.error(error)
  } finally {
    addUserForm.value = {}
    addUserDialogVisible.value = false
  }
}

async function deleteUser(id: string) {
  await api.users.deleteUser(id)
  await getUsers()
}

async function openEditUserRolesDialog(id: string) {
  userId.value = id
  roles.value = await api.roles.getRoles()
  userRoles.value = await api.users.getUserRoles(id)
  editUserRolesDialogVisible.value = true
}

async function editUserRoles() {
  await api.users.editUserRoles(userId.value, userRoles.value)
  editUserRolesDialogVisible.value = false
  await getUsers()
}
</script>
