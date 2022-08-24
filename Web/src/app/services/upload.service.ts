import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IApiResponseSingle } from '../models/IApiResponseSingle';
import { Helpers } from '../shared/Helpers';

@Injectable({ providedIn: 'root' })
export class UploadService {
  constructor(private httpClient: HttpClient) {}

  uploadImages(images: string[]): Observable<IApiResponseSingle> {
    const formData = new FormData();
    images.forEach((name) => {
      const imageData = sessionStorage.getItem(name);

      if (
        imageData &&
        imageData != 'null' &&
        !imageData?.startsWith('unchanged+')
      ) {
        formData.append('file', Helpers.dataToFile(imageData!, name));
      }
    });

    return this.httpClient.post<IApiResponseSingle>(
      `${environment.apiUrl}/Upload/UploadImage`,
      formData
    );
  }
}
