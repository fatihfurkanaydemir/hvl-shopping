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

  get isOnlyCustomer() {
    return this.roles.find((r) => r === 'Customer') && this.roles.length === 1;
  }

  get isOnlySeller() {
    return this.roles.find((r) => r === 'Seller') && this.roles.length === 1;
  }

  get isAdmin() {
    return this.roles.find((r) => r === 'Admin');
  }
}
