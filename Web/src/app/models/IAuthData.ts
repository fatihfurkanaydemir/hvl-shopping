export interface IAuthData {
  id: string;
  email: string;
  roles: string[];
  jwToken: string;
  expires: number;
}
