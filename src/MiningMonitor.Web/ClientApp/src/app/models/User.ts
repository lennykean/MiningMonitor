export interface User {
  username: string;
  email: string;
  password?: string;
}

export interface UserListItem extends User {
  isCurrentUser: boolean;
}
