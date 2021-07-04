import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { NgxSpinnerService } from "ngx-spinner";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  pdfSrc = "https://vadimdez.github.io/ng2-pdf-viewer/assets/pdf-test.pdf";

  busy: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string,
    private spinner: NgxSpinnerService) {
  }

  generateReport(): void {
    this.spinner.show();

    this.downloadPDF().subscribe(
      (res) => {
        var fileURL = URL.createObjectURL(res);
        this.pdfSrc = fileURL;
        this.spinner.hide();
      }
    );
  }

  downloadPDF(): any {
    return this.http.get(this.baseUrl + 'report', { responseType: 'blob' as 'json' })
      .pipe(map(res => {
        return new Blob([res as Blob], { type: 'application/pdf', });
      }));
  }
}
