import { StatusEnum } from '../enum/user-status-enum';
import { UserTypeEnum } from '../enum/user-type-enum';

export interface User {
  userId: number;
  userName: string;
  password: string;
  type: UserTypeEnum;
  status: StatusEnum;
}
