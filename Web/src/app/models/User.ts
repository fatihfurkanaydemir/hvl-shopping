export class User {
  constructor(
    public identityId: string,
    public email: string,
    public roles: string[],
    private _expires: number,
    private _token: string
  ) {}

  get token() {
    if (!this._expires || new Date() > new Date(this._expires * 1000))
      return null;
    return this._token;
  }

  hasRole(role: string) {
    return this.roles.find((r) => r === role);
  }
}
