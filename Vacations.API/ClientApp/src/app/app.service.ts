import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
    HttpResponse,
    HttpErrorResponse,
    HttpClient,
  } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import 'rxjs/add/observable/throw'
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import { ToastrService } from '../../node_modules/ngx-toastr';

@Injectable()
export class MyDataService implements OnInit{
    constructor(private http: HttpClient, private router: Router) { }

    ngOnInit() {
    }

    get<T>(url: string): Observable<T> {
        return this.http.get<T>(url);
    }

    post<T>(url: string, body: string): Observable<T> {
        return this.http.post<T>(url, body);
    }

    put<T>(url: string, body: string): Observable<T> {
        return this.http.put<T>(url, body);
    }

    delete<T>(url: string): Observable<T> {
        return this.http.delete<T>(url);
    }

    patch<T>(url: string, body: string): Observable<T> {
        return this.http.patch<T>(url, body);
    }
}


@Injectable()
export class MyFirstInterceptor implements HttpInterceptor {

        constructor(private router: Router, private toaster: ToastrService){ }

        intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        req = req.clone({ headers: req.headers.set('Content-Type', 'application/json')});

        req = req.clone({ headers: req.headers.set('Accept', 'application/json')});

        if (localStorage.getItem('token') && !req.headers.has('Authorization')) {
            req = req.clone({ headers: req.headers.set('Authorization', 'Bearer ' + localStorage.getItem('token'))});
        }

        console.log("Inter on");

        return next.handle(req).do((event: HttpEvent<any>) => 
        {
            if (event instanceof HttpResponse) {
                console.log("Inter HttpResponse");
              }
        }, (err: any) => {
            console.log("Inter in");
            if (err instanceof HttpErrorResponse) {
                if(err.status === 401)
                {
                    // this.router.navigate(["/auth"]);
                }
                if(err.status === 403)
                {
                    // this.router.navigate(["/main"]);
                }
                console.log("Inter toaster");
                this.toaster.error(err.message, err.status.toString());
                return Observable.throw(err);
            }
          });
    }
}