export interface IApiResponse {
  errors: any;
  pageNumber: number;
  pageSize: number;
  succeeded: boolean;
  message: string;
  dataCount: number;
  data: any[];
}
