export interface IApiResponse {
  errors: any;
  pageNumber: number;
  pageSize: number;
  succeeded: boolean;
  message: string;
  data: any[];
}
