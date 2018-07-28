import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { environment } from '../../../environments/environment.prod';

@Injectable()
export class ImageService {
    constructor (private http: HttpClient) { }

    postFile(url: string, file: File): Observable<File> {
        let formData:FormData = new FormData();
        formData.append('uploadFile', file, file.name);
        let headers = new HttpHeaders();
        /** In Angular 5, including the header Content-Type can invalidate your request */
        headers.append('Content-Type', 'multipart/form-data');

        return this.http.post<File>(url, formData, { headers: headers });
    }

    getImgUrl(): Observable<string> {
        let requestUrl = 'http://localhost:2076/api' + '/images/current';
        return this.http.get<string>(`${requestUrl}`);
    }
}

