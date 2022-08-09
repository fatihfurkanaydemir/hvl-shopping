export const ValidationPatterns = {
  phoneNumber: '^[+]?\\d{10,12}$',
  password:
    '^(?=.{6,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[/@/#/$/%/^/./&/+/=/*/-]).*$',
};
