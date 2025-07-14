import { LocationStrategy, PathLocationStrategy, PlatformLocation } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CleanUrlStrategy extends PathLocationStrategy {
  private currentUrl: string = '/';
  private readonly STORAGE_KEY = 'current_route';

  constructor(platformLocation: PlatformLocation) {
    super(platformLocation);
    const savedRoute = localStorage.getItem(this.STORAGE_KEY);
    if (savedRoute) {
      this.currentUrl = savedRoute;
    }
  }

  override pushState(state: any, title: string, url: string, queryParams: string = ''): void {
    this.currentUrl = url;
    localStorage.setItem(this.STORAGE_KEY, url);
    window.history.pushState(state, title, '/');
  }

  override replaceState(state: any, title: string, url: string, queryParams: string = ''): void {
    this.currentUrl = url;
    localStorage.setItem(this.STORAGE_KEY, url);
    window.history.replaceState(state, title, '/');
  }

  override prepareExternalUrl(internal: string): string {
    return this.currentUrl;
  }

  override path(): string {
    return '/';
  }

  override onPopState(fn: (event: PopStateEvent) => void): void {
    window.addEventListener('popstate', (event) => {
      window.history.pushState(event.state, '', '/');
      fn(event);
    });
  }

  getCurrentRoute(): string {
    return this.currentUrl;
  }

  clearSavedRoute(): void {
    localStorage.removeItem(this.STORAGE_KEY);
    this.currentUrl = '/';
  }
} 