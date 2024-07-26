import { ChangeDetectorRef, Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { UserDetail } from './models/user-details.model';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'PhoneBookApp';
  isAuthenticated:boolean=false;
  Username: string | null | undefined;
  imageUrl: string | ArrayBuffer | null = null;
  user:UserDetail={
    firstName: '',
    lastName: '',
    loginId: '',
    contactNumber: '',
    fileName: null,
    imageByte: '',
    email: '',
    userId: 0
  }
private userSubscription: Subscription | undefined

  constructor(private authService: AuthService, private cdr: ChangeDetectorRef, private route:ActivatedRoute){}
  ngOnInit(): void {
    this.userSubscription = this.authService.getUsername().subscribe((username: string | null | undefined) => {
      this.Username = username;
      if (this.Username) {
        this.getUser();
      }
      this.cdr.detectChanges(); //Manually trigger change detection.
    });
 
    this.authService.isAuthenticated().subscribe((authState: boolean) => {
      this.isAuthenticated=authState;
      this.cdr.detectChanges(); //Manually trigger change detection.
    });
    // Subscribe to profile updated event
    this.userSubscription = this.authService.onProfileUpdated().subscribe(() => {
      this.getUser();
    });
  }
    ngOnDestroy(): void {
      // Unsubscribe to prevent memory leaks
      if (this.userSubscription) {
        this.userSubscription.unsubscribe();
      }
  }
  signOut() {
    this.authService.SignOut();
  }
  getUser()
  {
    const loginId = this.Username;
    console.log(loginId);
    this.authService.getUserByLoginId(loginId).subscribe({
      next: (response) => {
        if (response.success) {
          this.user = response.data;
          if (this.user.imageByte) {
            this.imageUrl = 'data:image/jpeg;base64,' + this.user.imageByte;
          } else {
            this.imageUrl = null;
          }
        } else {
          console.error('Failed to fetch contact', response.message);
        }
      },
      error: (error) => {
        console.error('Failed to fetch contact', error);
      },
    });
  }
}
