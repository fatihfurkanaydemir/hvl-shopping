import { ICategory } from './ICategory';
import { IImage } from './IImage';

export interface IProductCreate {
  name: string;
  code: string;
  description: string;
  images: IImage[];
  inStock: number;
  categoryId: number;
}
