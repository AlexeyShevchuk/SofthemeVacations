import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class ImageService {
    constructor (private http: HttpClient) { }

    postFile(url: string, fileToUpload: File) {
        return this.http.post<File>(url, fileToUpload)
    }
}

