import { IImage } from './IImage';

export interface IProductUpdate {
  id: number;
  name: string;
  code: string;
  description: string;
  images: IImage[];
  inStock: number;
  categoryId: number;
}
